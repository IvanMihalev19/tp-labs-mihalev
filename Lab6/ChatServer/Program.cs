using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

const int Port = 5555;

var logLock = new object();
const string LogFile = "chat.log";

void Log(string message)
{
    string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
    Console.WriteLine(line);
    lock (logLock)
    {
        try { File.AppendAllText(LogFile, line + Environment.NewLine, Encoding.UTF8); }
        catch { }
    }
}

var clients = new ConcurrentDictionary<string, StreamWriter>(StringComparer.OrdinalIgnoreCase);

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Log($"Сервер запущен на порту {Port}. Ctrl+C — остановка.");

while (true)
{
    TcpClient tcp = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(tcp);
}

async Task HandleClientAsync(TcpClient tcp)
{
    string? nick = null;
    StreamWriter? writer = null;
    var endpoint = tcp.Client.RemoteEndPoint?.ToString() ?? "?";

    try
    {
        var stream = tcp.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);
        writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        while (true)
        {
            string? requested = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(requested))
            {
                await writer.WriteLineAsync("ERROR Ник не может быть пустым. Введите ник:");
                continue;
            }

            requested = requested.Trim();
            if (clients.TryAdd(requested, writer))
            {
                nick = requested;
                await writer.WriteLineAsync("OK");
                break;
            }
            await writer.WriteLineAsync("ERROR Ник уже занят. Введите другой:");
        }

        Log($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
        await BroadcastAsync($"*** {nick} вошёл в чат ***");

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            if (line.Equals("/exit", StringComparison.OrdinalIgnoreCase)) break;

            string msg = $"[{DateTime.Now:HH:mm:ss}] {nick}: {line}";
            Log(msg);
            await BroadcastAsync(msg);
            if (line.Equals("/list", StringComparison.OrdinalIgnoreCase))
            {
                var list = string.Join(", ", clients.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase));
                await writer.WriteLineAsync($"*** Онлайн ({clients.Count}): {list} ***");
                continue;
            }
            if (line.StartsWith("/w ", StringComparison.OrdinalIgnoreCase))
            {
                await HandlePrivateAsync(nick!, line, writer);
                continue;
            }
        }
    }
    catch (IOException) { }
    finally
    {
        if (nick != null)
        {
            clients.TryRemove(nick, out _);
            Log($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
            await BroadcastAsync($"*** {nick} покинул чат ***");
        }
        try { writer?.Dispose(); } catch { }
        try { tcp.Close(); } catch { }
    }
}

async Task BroadcastAsync(string message)
{
    foreach (var kv in clients)
    {
        try { await kv.Value.WriteLineAsync(message); }
        catch (IOException) { }
    }
}
async Task HandlePrivateAsync(string from, string line, StreamWriter senderWriter)
{
    string[] parts = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length < 3)
    {
        await senderWriter.WriteLineAsync("*** Использование: /w ник текст ***");
        return;
    }

    string targetNick = parts[1];
    string text = parts[2];

    if (clients.TryGetValue(targetNick, out var targetWriter))
    {
        string msgToTarget = $"[{DateTime.Now:HH:mm:ss}] (личное от {from}): {text}";
        string msgToSender = $"[{DateTime.Now:HH:mm:ss}] (личное для {targetNick}): {text}";
        try { await targetWriter.WriteLineAsync(msgToTarget); } catch { }
        try { await senderWriter.WriteLineAsync(msgToSender); } catch { }
    }
    else
    {
        await senderWriter.WriteLineAsync($"*** Пользователь '{targetNick}' не найден ***");
    }
}
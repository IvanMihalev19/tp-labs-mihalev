using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

const int Port = 5555;

var clients = new ConcurrentDictionary<string, StreamWriter>(StringComparer.OrdinalIgnoreCase);

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Сервер запущен на порту {Port}. Ctrl+C — остановка.");

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

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
        await BroadcastAsync($"*** {nick} вошёл в чат ***");

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            if (line.Equals("/exit", StringComparison.OrdinalIgnoreCase)) break;

            string msg = $"[{DateTime.Now:HH:mm:ss}] {nick}: {line}";
            Console.WriteLine(msg);
            await BroadcastAsync(msg);
            if (line.Equals("/list", StringComparison.OrdinalIgnoreCase))
            {
                var list = string.Join(", ", clients.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase));
                await writer.WriteLineAsync($"*** Онлайн ({clients.Count}): {list} ***");
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
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
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
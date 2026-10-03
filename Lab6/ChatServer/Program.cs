using System.Net;
using System.Net.Sockets;
using System.Text;

const int Port = 5555;

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
    var endpoint = tcp.Client.RemoteEndPoint?.ToString() ?? "?";
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Подключился {endpoint}");

    try
    {
        var stream = tcp.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);
        var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {endpoint}: {line}");
            await writer.WriteLineAsync($"Echo: {line}");
        }
    }
    catch (IOException) { }
    finally
    {
        tcp.Close();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {endpoint} отключился");
    }
}
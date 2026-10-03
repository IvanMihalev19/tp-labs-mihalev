using System.Net.Sockets;
using System.Text;

Console.Write("Адрес сервера (Enter = localhost): ");
string host = Console.ReadLine() is { Length: > 0 } h ? h.Trim() : "localhost";

using var client = new TcpClient();
try
{
    Console.WriteLine($"Подключение к {host}:5555...");
    await client.ConnectAsync(host, 5555);
}
catch (SocketException)
{
    Console.WriteLine("Ошибка: сервер недоступен.");
    return;
}

var stream = client.GetStream();
var reader = new StreamReader(stream, Encoding.UTF8);
var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

Console.WriteLine("Подключено. Пишите сообщения, /exit — выход.");

_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException) { }
    Console.WriteLine("*** Соединение потеряно. ***");
});

while (true)
{
    string? msg = Console.ReadLine();
    if (msg == null) break;
    await writer.WriteLineAsync(msg);
    if (msg == "/exit") break;
}
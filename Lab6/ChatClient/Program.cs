using System.Net.Sockets;
using System.Text;

Console.Write("Адрес сервера (Enter = localhost): ");
string host = Console.ReadLine() is { Length: > 0 } h ? h.Trim() : "localhost";

Console.Write("Порт (Enter = 5555): ");
string? portStr = Console.ReadLine();
int port = 5555;
if (!string.IsNullOrWhiteSpace(portStr) && int.TryParse(portStr, out int pp) && pp is > 0 and < 65536)
    port = pp;

using var client = new TcpClient();
try
{
    Console.WriteLine($"Подключение к {host}:{port}...");
    await client.ConnectAsync(host, port);
}
catch (SocketException)
{
    Console.WriteLine("Ошибка: сервер недоступен.");
    return;
}

var stream = client.GetStream();
var reader = new StreamReader(stream, Encoding.UTF8);
var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

while (true)
{
    Console.Write("Ваш ник: ");
    string? nick = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(nick))
    {
        Console.WriteLine("Ник не может быть пустым.");
        continue;
    }

    await writer.WriteLineAsync(nick);
    string? response = await reader.ReadLineAsync();
    if (response == null) { Console.WriteLine("Сервер закрыл соединение."); return; }
    if (response.StartsWith("OK")) break;
    Console.WriteLine(response.StartsWith("ERROR ") ? response[6..] : response);
}

Console.WriteLine("Подключено! Команды: /list, /exit\n");

_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException) { }
    Console.WriteLine("\n*** Соединение потеряно. ***");
});

while (true)
{
    string? msg = Console.ReadLine();
    if (msg == null) break;
    await writer.WriteLineAsync(msg);
    if (msg.Equals("/exit", StringComparison.OrdinalIgnoreCase)) break;
}
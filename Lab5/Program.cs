using System.Diagnostics;

const long N = 100_000_000;

Console.Write("Введите количество потоков (1 — последовательно): ");
if (!int.TryParse(Console.ReadLine(), out int threadCount) || threadCount < 1)
{
    Console.WriteLine("Некорректное число потоков. Используется 1.");
    threadCount = 1;
}

Console.WriteLine($"\nКоличество точек: {N:N0}");
Console.WriteLine($"Ядер процессора: {Environment.ProcessorCount}");
Console.WriteLine($"Потоков: {threadCount}\n");

var sw = Stopwatch.StartNew();
double piSeq = CalculatePiSequential(N);
sw.Stop();
long timeSeq = sw.ElapsedMilliseconds;
Console.WriteLine($"Последовательно: π ≈ {piSeq:F10}, время = {timeSeq} мс");

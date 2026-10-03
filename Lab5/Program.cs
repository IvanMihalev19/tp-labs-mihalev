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

sw.Restart();
double piPar = CalculatePiParallel(N, threadCount);
sw.Stop();
long timePar = sw.ElapsedMilliseconds;
Console.WriteLine($"Параллельно:     π ≈ {piPar:F10}, время = {timePar} мс");

double speedup = timeSeq > 0 ? (double)timeSeq / timePar : 0;
Console.WriteLine($"\nУскорение: {speedup:F2}x");
Console.WriteLine($"Истинное π: {Math.PI:F10}");
Console.WriteLine($"Ошибка последовательно: {Math.Abs(piSeq - Math.PI):E}");
Console.WriteLine($"Ошибка параллельно:     {Math.Abs(piPar - Math.PI):E}");

if (Math.Abs(piSeq - Math.PI) < 0.001 && Math.Abs(piPar - Math.PI) < 0.001)
    Console.WriteLine("\nОбе версии дают корректный результат (в пределах статистической погрешности).");
else
    Console.WriteLine("\nВнимание: одна из версий дала слишком большую ошибку.");
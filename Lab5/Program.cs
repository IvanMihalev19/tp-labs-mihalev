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

static double CalculatePiSequential(long totalPoints)
{
    var rnd = new Random(42);
    long hits = 0;

    for (long i = 0; i < totalPoints; i++)
    {
        double x = rnd.NextDouble();
        double y = rnd.NextDouble();
        if (x * x + y * y <= 1.0)
            hits++;
    }

    return 4.0 * hits / totalPoints;
}

static double CalculatePiParallel(long totalPoints, int threadCount)
{
    if (threadCount == 1)
        return CalculatePiSequential(totalPoints);

    long pointsPerThread = totalPoints / threadCount;
    long remainder = totalPoints % threadCount;

    var threads = new Thread[threadCount];
    var hits = new long[threadCount];

    for (int t = 0; t < threadCount; t++)
    {
        int threadIndex = t;
        long count = pointsPerThread + (threadIndex < remainder ? 1 : 0);
        int seed = 42 + threadIndex * 1000;

        threads[t] = new Thread(() =>
        {
            var rnd = new Random(seed);
            long localHits = 0;

            for (long i = 0; i < count; i++)
            {
                double x = rnd.NextDouble();
                double y = rnd.NextDouble();
                if (x * x + y * y <= 1.0)
                    localHits++;
            }

            hits[threadIndex] = localHits;
        });
    }

    foreach (var th in threads)
        th.Start();

    foreach (var th in threads)
        th.Join();

    long totalHits = 0;
    foreach (var h in hits)
        totalHits += h;

    return 4.0 * totalHits / totalPoints;
}
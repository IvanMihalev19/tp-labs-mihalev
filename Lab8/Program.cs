using System.Diagnostics;
using System.Text;

const int N = 30_000;

var swTotal = Stopwatch.StartNew();

// --- 1. Генерация данных ---
var rnd = new Random(42);                       // фиксированное зерно: результат воспроизводим
var buyers = new List<string>();
var amounts = new List<int>();
for (int i = 0; i < N; i++)
{
    buyers.Add("user" + rnd.Next(0, 5000));
    amounts.Add(rnd.Next(1, 10_000));
}

// --- 2. Список уникальных покупателей ---
var sw = Stopwatch.StartNew();
var unique = new HashSet<string>(buyers);
Console.WriteLine($"Уникальных покупателей: {unique.Count}  ({sw.ElapsedMilliseconds} мс)");

// --- 3. Текстовый отчёт ---
sw.Restart();
var sb = new StringBuilder(buyers.Count * 20);
for (int i = 0; i < buyers.Count; i++)
{
    sb.Append(buyers[i]).Append(';')
      .Append(amounts[i]).AppendLine();   // УЗКОЕ МЕСТО №2
}
string report = sb.ToString();
Console.WriteLine($"Отчёт: {report.Length} символов  ({sw.ElapsedMilliseconds} мс)");

// --- 4. Сортировка сумм ---
sw.Restart();
var sorted = new List<int>(amounts);
sorted.Sort();
Console.WriteLine($"Медианная сумма: {sorted[sorted.Count / 2]}  ({sw.ElapsedMilliseconds} мс)");

// --- 5. Сумма покупок каждого покупателя ---
sw.Restart();
var totalsDict = new Dictionary<string, long>();
for (int i = 0; i < buyers.Count; i++)
{
    if (totalsDict.TryGetValue(buyers[i], out long total))
        totalsDict[buyers[i]] = total + amounts[i];
    else
        totalsDict[buyers[i]] = amounts[i];
}

var maxBuyer = totalsDict.MaxBy(kv => kv.Value).Key;
Console.WriteLine($"Максимум потратил: {maxBuyer}  ({sw.ElapsedMilliseconds} мс)");

Console.WriteLine($"ИТОГО: {swTotal.ElapsedMilliseconds} мс");
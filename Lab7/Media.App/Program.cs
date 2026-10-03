using ConsoleTables;
using Media.Core;

const string CatalogFile = "media_catalog.txt";
IMediaRepository repository = args.Contains("--memory")
    ? new InMemoryMediaRepository()
    : new FileMediaRepository(CatalogFile);

var catalog = new MediaCatalogService(repository);

Console.WriteLine("=== Медиа-каталог ===");
Console.WriteLine($"Хранилище: {(repository is FileMediaRepository ? CatalogFile : "память")}");
PrintHelp();

while (true)
{
    Console.Write("\n> ");
    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(input)) continue;

    var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLowerInvariant();
    var arg = parts.Length > 1 ? parts[1] : null;

    try
    {
        switch (cmd)
        {
            case "scan":
            case "s":
                if (string.IsNullOrWhiteSpace(arg))
                {
                    Console.WriteLine("Использование: scan <путь_к_папке>");
                    break;
                }
                var count = catalog.ScanDirectory(arg);
                Console.WriteLine($"Найдено медиафайлов: {count}. Всего в каталоге: {catalog.AllFiles.Count}");
                break;

            case "list":
            case "l":
                PrintFiles(catalog.AllFiles, "Все файлы");
                break;

            case "audio":
            case "a":
                PrintFiles(catalog.GetByType(MediaType.Audio), "Аудио");
                break;

            case "video":
            case "v":
                PrintFiles(catalog.GetByType(MediaType.Video), "Видео");
                break;

            case "image":
            case "i":
                PrintFiles(catalog.GetByType(MediaType.Image), "Изображения");
                break;

            case "search":
            case "find":
            case "f":
                if (string.IsNullOrWhiteSpace(arg))
                {
                    Console.WriteLine("Использование: search <ключевое_слово>");
                    break;
                }
                PrintFiles(catalog.Search(arg), $"Поиск: «{arg}»");
                break;

            case "stats":
            case "st":
                PrintStats(catalog.GetStats());
                break;

            case "clear":
            case "c":
                catalog.Clear();
                Console.WriteLine("Каталог очищен.");
                break;

            case "reload":
            case "r":
                catalog.LoadFromRepository();
                Console.WriteLine($"Загружено из хранилища: {catalog.AllFiles.Count} файлов.");
                break;

            case "help":
            case "h":
            case "?":
                PrintHelp();
                break;

            case "exit":
            case "quit":
            case "q":
            case "0":
                return;

            default:
                Console.WriteLine("Неизвестная команда. Введите help.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

static void PrintHelp()
{
    Console.WriteLine("""
        Команды:
          scan <папка>     — сканировать папку (рекурсивно)
          list / l         — показать все файлы
          audio / a        — только аудио
          video / v        — только видео
          image / i        — только изображения
          search <слово>   — поиск по имени/пути
          stats / st       — статистика
          clear / c        — очистить каталог
          reload / r       — перезагрузить из файла
          help / h         — справка
          exit / q / 0     — выход
        """);
}

static void PrintFiles(IReadOnlyList<MediaFile> files, string title)
{
    Console.WriteLine($"\n--- {title} ({files.Count}) ---");
    if (files.Count == 0)
    {
        Console.WriteLine("(пусто)");
        return;
    }

    var table = new ConsoleTable("№", "Имя", "Тип", "Размер", "Путь");
    int i = 1;
    foreach (var f in files.Take(50))
    {
        table.AddRow(i++, f.FileName, f.Type, FormatSize(f.SizeBytes), Truncate(f.FullPath, 50));
    }
    table.Write(Format.Minimal);

    if (files.Count > 50)
        Console.WriteLine($"... и ещё {files.Count - 50} файлов");
}

static void PrintStats(CatalogStats s)
{
    Console.WriteLine("\n--- Статистика каталога ---");
    var table = new ConsoleTable("Раздел", "Кол-во", "Размер");
    table.AddRow("Аудио", s.AudioCount, FormatSize(s.AudioSize));
    table.AddRow("Видео", s.VideoCount, FormatSize(s.VideoSize));
    table.AddRow("Изображения", s.ImageCount, FormatSize(s.ImageSize));
    table.AddRow("ВСЕГО", s.TotalFiles, FormatSize(s.TotalSizeBytes));
    table.Write(Format.Minimal);
}

static string FormatSize(long bytes)
{
    if (bytes < 1024) return $"{bytes} B";
    if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
    if (bytes < 1024L * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
    return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
}

static string Truncate(string s, int max) =>
    s.Length <= max ? s : "…" + s[^(max - 1)..];
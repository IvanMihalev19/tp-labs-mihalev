using System.Text;

namespace Lab3;

class Program
{
    static void Main()
    {

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Лабораторная работа 3. Вариант 2: Просмотр файла с кодировкой ===");
        Console.WriteLine();

        while (true)
        {
            try
            {
                ShowMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ViewFile();
                        break;
                    case "2":
                        ShowHelp();
                        break;
                    case "0":
                        Console.WriteLine("Выход.");
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню. Повторите ввод.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
    }
    static void ShowMenu()
    {
        Console.WriteLine("Меню:");
        Console.WriteLine("  1 — Просмотреть файл");
        Console.WriteLine("  2 — Справка по кодировкам");
        Console.WriteLine("  0 — Выход");
        Console.Write("Выберите пункт: ");
    }

    static void ShowHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Доступные кодировки:");
        Console.WriteLine("  1. UTF-8        — современный стандарт (по умолчанию в .NET)");
        Console.WriteLine("  2. Windows-1251 — кириллица для Windows (часто в старых файлах)");
        Console.WriteLine("  3. CP866        — кириллица DOS / OEM");
        Console.WriteLine();
        Console.WriteLine("Если файл сохранён в одной кодировке, а прочитан в другой,");
        Console.WriteLine("текст превращается в «кракозябры».");
        Console.WriteLine();
        Console.WriteLine("Рекомендация для защиты: сохраните файл в Windows-1251 (вариант 1),");
        Console.WriteLine("затем откройте его сначала в Windows-1251 (корректно),");
        Console.WriteLine("потом в UTF-8 (кракозябры) — и наоборот.");
    }
    static void ViewFile()
    {
        Console.WriteLine();
        Console.Write("Введите полный путь к файлу: ");
        string? path = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Путь не может быть пустым.");
            return;
        }

        // Убираем кавычки, если пользователь скопировал путь с ними
        path = path.Trim('"', '\'');

        if (!File.Exists(path))
        {
            Console.WriteLine($"Ошибка: файл не найден — «{path}»");
            return;
        }

        Encoding? encoding = ChooseEncoding();
        if (encoding is null)
            return;

        try
        {
            string content = FileViewer.ReadAllText(path, encoding);

            Console.WriteLine();
            Console.WriteLine("---------- Содержимое файла ----------");
            Console.WriteLine(content);
            Console.WriteLine("--------------------------------------");

            var info = new FileInfo(path);
            Console.WriteLine();
            Console.WriteLine($"Файл: {info.FullName}");
            Console.WriteLine($"Размер: {info.Length} байт");
            Console.WriteLine($"Кодировка: {encoding.EncodingName} (CodePage {encoding.CodePage})");
            Console.WriteLine($"Дата изменения: {info.LastWriteTime}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Ошибка: нет прав на чтение файла.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении: {ex.Message}");
        }
    }
    static Encoding? ChooseEncoding()
    {
        Console.WriteLine();
        Console.WriteLine("Выберите кодировку:");
        Console.WriteLine("  1 — UTF-8");
        Console.WriteLine("  2 — Windows-1251");
        Console.WriteLine("  3 — CP866");
        Console.Write("Ваш выбор: ");

        string? choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                return Encoding.UTF8;
            case "2":
                return Encoding.GetEncoding(1251);
            case "3":
                return Encoding.GetEncoding(866);
            default:
                Console.WriteLine("Неверный выбор кодировки.");
                return null;
        }
    }
}

static class FileViewer
{

    public static string ReadAllText(string path, Encoding encoding)
    {
        using var reader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks: true);
        return reader.ReadToEnd();
    }

    public static IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
        using var reader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks: true);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}
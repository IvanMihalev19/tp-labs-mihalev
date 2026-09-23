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

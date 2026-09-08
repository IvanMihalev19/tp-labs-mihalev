using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Лабораторная работа 1");
            Console.WriteLine("1 — Факториал");
            Console.WriteLine("2 — Числа Фибоначчи");
            Console.WriteLine("3 — Значение функции");
            Console.WriteLine("4 — Ряд Тейлора");
            Console.Write("Выберите задание (1-4): ");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Ошибка: введите число от 1 до 4.");
                return;
            }

            switch (choice)
            {
                case 1: Task1(); break;
                case 2: Task2(); break;
            }
        }
        static void Task1()
        {
            Console.Write("Введите n (0..20): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 0 || n > 20)
            {
                Console.WriteLine("Ошибка: нужно целое число от 0 до 20.");
                return;
            }

            Console.WriteLine($"{n}! = {Factorial(n)}");
        }

        static long Factorial(int n)
        {
            long result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }
        static void Task2()
        {
            Console.Write("Введите n (>= 0): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
            {
                Console.WriteLine("Ошибка: нужно целое число >= 0.");
                return;
            }

            Console.WriteLine(FibonacciSequence(n));
        }

        static string FibonacciSequence(int n)
        {
            if (n == 0) return "0";
            if (n == 1) return "0, 1";

            long a = 0, b = 1;
            string result = "0, 1";

            for (int i = 2; i <= n; i++)
            {
                long next = a + b;
                result += $", {next}";
                a = b;
                b = next;
            }
            return result;
        }


    }
}
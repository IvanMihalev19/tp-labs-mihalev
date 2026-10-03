namespace Lab4.Core;

public static class MathAlgorithms
{
    public static long Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "n не может быть отрицательным");
        if (n > 20)
            throw new ArgumentOutOfRangeException(nameof(n), "n > 20 вызывает переполнение long");

        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    public static IReadOnlyList<long> Fibonacci(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "count не может быть отрицательным");

        var result = new List<long>(count);
        long a = 0, b = 1;
        for (int i = 0; i < count; i++)
        {
            result.Add(a);
            (a, b) = (b, a + b);
        }
        return result;
    }
    public static double CalculateFunction(double x)
    {
        if (x <= 0)
            throw new ArgumentOutOfRangeException(nameof(x), "x должен быть > 0 (логарифм)");

        if (Math.Abs(x + 3) < 1e-12)
            throw new ArgumentException("Деление на ноль: x = -3", nameof(x));

        double underSqrt = Math.Cos(x - 32) * (x - 3) / (x + 3);
        if (underSqrt < 0)
            throw new ArgumentException("Подкоренное выражение отрицательное", nameof(x));

        return Math.Sqrt(underSqrt) - Math.Log(x) / Math.Log(5);
    }

    public static (double Sum, int Terms) CosTaylor(double x, double eps = 1e-6)
    {
        if (eps <= 0)
            throw new ArgumentOutOfRangeException(nameof(eps), "eps должен быть > 0");

        double sum = 0;
        double term = 1;
        int n = 0;
        int terms = 0;
        int sign = 1;

        while (Math.Abs(term) > eps)
        {
            sum += sign * term;
            terms++;
            term *= x * x / ((n + 1.0) * (n + 2.0));
            n += 2;
            sign = -sign;
        }
        return (sum, terms);
    }
}
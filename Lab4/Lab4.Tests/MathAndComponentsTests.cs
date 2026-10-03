using Lab4.Core;

namespace Lab4.Tests;

public class MathAlgorithmsTests
{

    [Fact]
    public void Factorial_OfZero_ReturnsOne()
    {
        long result = MathAlgorithms.Factorial(0);
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    [InlineData(20, 2432902008176640000)]
    public void Factorial_ValidInput_ReturnsExpected(int n, long expected)
    {
        long result = MathAlgorithms.Factorial(n);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(21)]
    [InlineData(25)]
    public void Factorial_OutOfRange_ThrowsArgumentOutOfRange(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Factorial(n));
    }

    [Fact]
    public void Fibonacci_ZeroCount_ReturnsEmpty()
    {
        var result = MathAlgorithms.Fibonacci(0);
        Assert.Empty(result);
    }

    [Fact]
    public void Fibonacci_FirstSix_AreCorrect()
    {
        var result = MathAlgorithms.Fibonacci(6);
        Assert.Equal(new long[] { 0, 1, 1, 2, 3, 5 }, result);
    }

    [Theory]
    [InlineData(1, new long[] { 0 })]
    [InlineData(2, new long[] { 0, 1 })]
    [InlineData(7, new long[] { 0, 1, 1, 2, 3, 5, 8 })]
    public void Fibonacci_ValidCount_ReturnsExpectedSequence(int count, long[] expected)
    {
        var result = MathAlgorithms.Fibonacci(count);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Fibonacci_NegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Fibonacci(-1));
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(5.0)]
    [InlineData(10.5)]
    public void CalculateFunction_ValidX_ReturnsFiniteNumber(double x)
    {
        double result = MathAlgorithms.CalculateFunction(x);
        Assert.True(double.IsFinite(result));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5.5)]
    public void CalculateFunction_NonPositiveX_Throws(double x)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.CalculateFunction(x));
    }

    [Fact]
    public void CalculateFunction_XEqualsMinus3_Throws()
    {
        Assert.Throws<ArgumentException>(() => MathAlgorithms.CalculateFunction(-3));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(Math.PI / 2)]
    [InlineData(-0.7)]
    public void CosTaylor_MatchesMathCos(double x)
    {
        var (sum, _) = MathAlgorithms.CosTaylor(x);
        Assert.Equal(Math.Cos(x), sum, 1e-5);
    }

    [Fact]
    public void CosTaylor_ZeroEps_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.CosTaylor(1.0, 0));
    }

    [Fact]
    public void CosTaylor_NegativeEps_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.CosTaylor(1.0, -0.001));
    }
}
public class ComponentTests
{

    [Fact]
    public void Processor_ValidData_CreatedSuccessfully()
    {
        var cpu = new Processor("Ryzen 7", 35000m, 120, 8, 4.2);
        Assert.Equal("Ryzen 7", cpu.Name);
        Assert.Equal(35000m, cpu.Price);
        Assert.Equal(120, cpu.PowerConsumption);
        Assert.Equal(8, cpu.Cores);
        Assert.Equal(4.2, cpu.FrequencyGHz);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-0.01)]
    public void Processor_NegativePrice_Throws(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Processor("CPU", price, 100, 8, 3.5));
    }

    [Fact]
    public void Processor_ZeroCores_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Processor("CPU", 10000m, 100, 0, 3.5));
    }

    [Fact]
    public void Memory_EmptyType_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new Memory("RAM", 5000m, 5, 16, "  ", 3200));
    }

    [Fact]
    public void Storage_NegativeCapacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Storage("SSD", 8000m, 6, -500, "SSD", 3500));
    }


    [Fact]
    public void Computer_AddComponents_CalculatesTotalPriceAndPower()
    {
        var pc = new Computer("Gaming");
        var cpu = new Processor("Ryzen 7", 35000m, 120, 8, 4.2);
        var ram = new Memory("Kingston", 9000m, 5, 32, "DDR5", 6000);
        var ssd = new Storage("Samsung", 12000m, 6.5, 2000, "SSD", 7000);

        pc.AddComponent(cpu);
        pc.AddComponent(ram);
        pc.AddComponent(ssd);

        Assert.Equal(3, pc.Components.Count);
        Assert.Equal(56000m, pc.TotalPrice);
        Assert.Equal(131.5, pc.TotalPowerConsumption, 1);
    }

    [Fact]
    public void Computer_Empty_TotalPriceIsZero()
    {
        var pc = new Computer("Empty");
        Assert.Equal(0m, pc.TotalPrice);
        Assert.Equal(0.0, pc.TotalPowerConsumption);
        Assert.Empty(pc.Components);
    }

    [Fact]
    public void Computer_AddNull_Throws()
    {
        var pc = new Computer("Test");
        Assert.Throws<ArgumentNullException>(() => pc.AddComponent(null!));
    }

    [Fact]
    public void Computer_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Computer("   "));
    }

    [Fact]
    public void Component_ToString_ContainsNameAndPrice()
    {
        var cpu = new Processor("TestCPU", 10000m, 65, 6, 3.0);
        string text = cpu.ToString();
        Assert.Contains("TestCPU", text);
        Assert.Contains("Processor", text);
    }
}
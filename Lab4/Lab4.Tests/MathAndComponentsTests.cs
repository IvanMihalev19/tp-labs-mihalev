using Lab4.Core;

namespace Lab4.Tests;

public class MathAlgorithmsTests
{
    // ========== FACTORIAL ==========

    [Fact]
    public void Factorial_OfZero_ReturnsOne()
    {
        // Arrange + Act
        long result = MathAlgorithms.Factorial(0);
        // Assert
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
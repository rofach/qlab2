using System;
using Xunit;
using QLAB_2;

namespace QLAB_2.Tests;

public class CalculatorTests : IDisposable
{
    private readonly Calculator _calculator;

    public CalculatorTests()
    {
        _calculator = new Calculator();
    }

    public void Dispose()
    {
        _calculator.Reset();
    }

    [Fact(DisplayName = "Позитивний тест 1: Додавання двох додатних чисел повертає точну суму")]
    public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
    {
        double a = 12.5;
        double b = 7.5;
        double expected = 20.0;

        double actual = _calculator.Add(a, b);

        Assert.Equal(expected, actual, precision: 5);
    }

    [Fact(DisplayName = "Позитивний тест 2: Віднімання дійсних чисел обчислюється коректно")]
    public void Subtract_TwoNumbers_ReturnsCorrectDifference()
    {
        double a = 100.0;
        double b = 45.5;
        double expected = 54.5;

        double actual = _calculator.Subtract(a, b);

        Assert.Equal(expected, actual, precision: 5);
    }

    [Fact(DisplayName = "Позитивний тест 3: Ділення чисел з ненульовим дільником обчислюється коректно")]
    public void Divide_ValidOperands_ReturnsCorrectQuotient()
    {
        double dividend = 15.0;
        double divisor = 4.0;
        double expected = 3.75;

        double actual = _calculator.Divide(dividend, divisor);

        Assert.Equal(expected, actual, precision: 5);
    }

    [Theory(DisplayName = "Межовий випадок 1: Додавання нуля до будь-якого числа не змінює його значення")]
    [InlineData(0.0, 0.0, 0.0)]
    [InlineData(42.0, 0.0, 42.0)]
    [InlineData(0.0, -15.5, -15.5)]
    public void Add_WithZero_ReturnsOriginalNumber(double a, double b, double expected)
    {
        double actual = _calculator.Add(a, b);

        Assert.Equal(expected, actual, precision: 5);
    }

    [Fact(DisplayName = "Межовий випадок 2: Віднімання числа від самого себе завжди повертає 0")]
    public void Subtract_NumberFromItself_ReturnsZero()
    {
        double number = 987.654;

        double actual = _calculator.Subtract(number, number);

        Assert.Equal(0.0, actual, precision: 5);
    }

    [Fact(DisplayName = "Межовий випадок 3: Ділення нуля на будь-яке ненульове число дає нуль")]
    public void Divide_ZeroByNumber_ReturnsZero()
    {
        double actual = _calculator.Divide(0.0, 123.45);

        Assert.Equal(0.0, actual, precision: 5);
    }

    [Fact(DisplayName = "Виняток 1: Ділення на звичайний нуль (0.0) викидає DivideByZeroException")]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        double dividend = 42.0;
        double zeroDivisor = 0.0;

        var exception = Assert.Throws<DivideByZeroException>(() =>
        {
            _calculator.Divide(dividend, zeroDivisor);
        });

        Assert.Equal("Ділення на нуль неможливе.", exception.Message);
    }

    [Theory(DisplayName = "Виняток 2: Ділення на нескінченно мале число розцінюється як ділення на нуль")]
    [InlineData(1e-16)]
    [InlineData(-1e-16)]
    public void Divide_ByInfinitesimalDivisor_ThrowsDivideByZeroException(double infinitesimal)
    {
        Assert.Throws<DivideByZeroException>(() =>
        {
            _calculator.Divide(100.0, infinitesimal);
        });
    }

    [Fact(DisplayName = "assertAll (Assert.Multiple): Перевірка стану калькулятора після послідовності обчислень")]
    public void MultipleAssertions_SequentialCalculations_AllStateIsConsistent()
    {
        double step1 = _calculator.Add(10.0, 20.0);
        double step2 = _calculator.Subtract(step1, 5.0);
        double step3 = _calculator.Divide(step2, 5.0);

        Assert.Multiple(
            () => Assert.Equal(5.0, step3, precision: 5),
            () => Assert.Equal(5.0, _calculator.LastResult, precision: 5),
            () => Assert.Equal(3, _calculator.ExecutionCount),
            () => Assert.Equal(3, _calculator.History.Count),
            () => Assert.Contains("10 + 20 = 30", _calculator.History[0]),
            () => Assert.Contains("30 - 5 = 25", _calculator.History[1]),
            () => Assert.Contains("25 / 5 = 5", _calculator.History[2])
        );
    }
}

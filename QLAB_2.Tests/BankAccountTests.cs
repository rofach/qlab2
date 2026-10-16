using System;
using Xunit;
using QLAB_2;

namespace QLAB_2.Tests;

public class BankAccountTests : IDisposable
{
    private readonly BankAccount _account;

    public BankAccountTests()
    {
        _account = new BankAccount("UA1234567890", "Тарас Шевченко", 1000m);
    }

    public void Dispose()
    {
    }

    [Fact(DisplayName = "Позитивний тест 1: Успішне поповнення рахунку коректно збільшує баланс")]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        decimal depositAmount = 500m;
        decimal expectedBalance = 1500m;

        _account.Deposit(depositAmount);

        Assert.Equal(expectedBalance, _account.Balance);
    }

    [Fact(DisplayName = "Позитивний тест 2: Успішне зняття коштів коректно зменшує баланс")]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        decimal withdrawAmount = 400m;
        decimal expectedBalance = 600m;

        _account.Withdraw(withdrawAmount);

        Assert.Equal(expectedBalance, _account.Balance);
    }

    [Fact(DisplayName = "Межовий випадок 1: Зняття всієї доступної суми балансу залишає рівно 0")]
    public void Withdraw_ExactTotalBalance_SetsBalanceToZero()
    {
        _account.Withdraw(1000m);

        Assert.Equal(0m, _account.Balance);
    }

    [Fact(DisplayName = "Межовий випадок 2: Поповнення на мінімальну суму (0.01) коректно зараховується")]
    public void Deposit_MinimalFractionalAmount_IncreasesBalanceCorrectly()
    {
        decimal minimalAmount = 0.01m;
        decimal expectedBalance = 1000.01m;

        _account.Deposit(minimalAmount);

        Assert.Equal(expectedBalance, _account.Balance);
    }

    [Fact(DisplayName = "Межовий випадок 3: Створення рахунку з нульовим початковим балансом")]
    public void Constructor_ZeroInitialBalance_InitializesSuccessfully()
    {
        var zeroAccount = new BankAccount("UA0000000000", "Олесь Гончар", 0m);

        Assert.Equal(0m, zeroAccount.Balance);
    }

    [Fact(DisplayName = "Виняток 1: Спроба зняття суми, що перевищує баланс, викидає InvalidOperationException")]
    public void Withdraw_AmountGreaterThanBalance_ThrowsInvalidOperationException()
    {
        decimal excessiveAmount = 1500m;

        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            _account.Withdraw(excessiveAmount);
        });

        Assert.Contains("Недостатньо коштів", exception.Message);
    }

    [Theory(DisplayName = "Виняток 2: Поповнення на нульову або від'ємну суму викидає ArgumentOutOfRangeException")]
    [InlineData(0)]
    [InlineData(-50)]
    [InlineData(-0.01)]
    public void Deposit_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(decimal invalidAmount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _account.Deposit(invalidAmount);
        });
    }

    [Theory(DisplayName = "Виняток 3: Зняття нульової або від'ємної суми викидає ArgumentOutOfRangeException")]
    [InlineData(0)]
    [InlineData(-100)]
    public void Withdraw_ZeroOrNegativeAmount_ThrowsArgumentOutOfRangeException(decimal invalidAmount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _account.Withdraw(invalidAmount);
        });
    }

    [Theory(DisplayName = "Виняток 4: Створення рахунку з порожнім номером або власником викидає ArgumentException")]
    [InlineData("", "Іван")]
    [InlineData("   ", "Іван")]
    [InlineData("UA111", "")]
    [InlineData("UA111", "   ")]
    public void Constructor_InvalidParameters_ThrowsArgumentException(string accountNumber, string owner)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new BankAccount(accountNumber, owner, 100m);
        });
    }

    [Fact(DisplayName = "Виняток 5: Поповнення деактивованого рахунку викидає InvalidOperationException")]
    public void Deposit_DeactivatedAccount_ThrowsInvalidOperationException()
    {
        _account.Deactivate();

        Assert.Throws<InvalidOperationException>(() =>
        {
            _account.Deposit(100m);
        });
    }

    [Fact(DisplayName = "assertAll (Assert.Multiple): Перевірка узгодженості кількох властивостей рахунку після серії дій")]
    public void MultipleAssertions_AfterSeriesOfOperations_AllStateIsConsistent()
    {
        _account.Deposit(500m);
        _account.Withdraw(200m);

        Assert.Multiple(
            () => Assert.Equal(1300m, _account.Balance),
            () => Assert.Equal("UA1234567890", _account.AccountNumber),
            () => Assert.Equal("Тарас Шевченко", _account.Owner),
            () => Assert.True(_account.IsActive),
            () => Assert.Equal(3, _account.Transactions.Count),
            () => Assert.Contains("Поповнення", _account.Transactions[1]),
            () => Assert.Contains("Зняття", _account.Transactions[2])
        );
    }
}

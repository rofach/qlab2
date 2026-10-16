namespace QLAB_2;

public class BankAccount
{
    public string AccountNumber { get; }
    public string Owner { get; }
    public decimal Balance { get; private set; }
    public bool IsActive { get; private set; }
    public List<string> Transactions { get; } = new();

    public BankAccount(string accountNumber, string owner, decimal initialBalance = 0m)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Номер рахунку не може бути порожнім.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Власник рахунку не може бути порожнім.", nameof(owner));

        if (initialBalance < 0m)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "Початковий баланс не може бути від'ємним.");

        AccountNumber = accountNumber.Trim();
        Owner = owner.Trim();
        Balance = initialBalance;
        IsActive = true;
        Transactions.Add($"Рахунок відкрито з початковим балансом: {Balance:C}");
    }

    public void Deposit(decimal amount)
    {
        if (!IsActive)
            throw new InvalidOperationException("Операція неможлива: рахунок деактивовано.");

        if (amount <= 0m)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сума поповнення повинна бути строго більшою за нуль.");

        Balance += amount;
        Transactions.Add($"Поповнення: +{amount:C}; Новий баланс: {Balance:C}");
    }

    public void Withdraw(decimal amount)
    {
        if (!IsActive)
            throw new InvalidOperationException("Операція неможлива: рахунок деактивовано.");

        if (amount <= 0m)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сума зняття повинна бути строго більшою за нуль.");

        if (amount > Balance)
            throw new InvalidOperationException($"Недостатньо коштів на рахунку. Поточний баланс: {Balance:C}, спроба зняти: {amount:C}.");

        Balance -= amount;
        Transactions.Add($"Зняття: -{amount:C}; Новий баланс: {Balance:C}");
    }

    public void Deactivate()
    {
        IsActive = false;
        Transactions.Add("Рахунок деактивовано.");
    }

    public void Activate()
    {
        IsActive = true;
        Transactions.Add("Рахунок активовано.");
    }
}

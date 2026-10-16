using QLAB_2;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- Варіант 1: BankAccount ---");
var account = new BankAccount("UA1234567890", "Іван Франко", 1000m);
Console.WriteLine($"Рахунок: {account.AccountNumber}, Власник: {account.Owner}, Баланс: {account.Balance:C}");
account.Deposit(500m);
Console.WriteLine($"Після поповнення (+500): {account.Balance:C}");
account.Withdraw(300m);
Console.WriteLine($"Після зняття (-300): {account.Balance:C}");
Console.WriteLine();

Console.WriteLine("--- Варіант 4: Calculator ---");
var calc = new Calculator();
Console.WriteLine($"10 + 5 = {calc.Add(10, 5)}");
Console.WriteLine($"10 - 4 = {calc.Subtract(10, 4)}");
Console.WriteLine($"6 * 7 = {calc.Multiply(6, 7)}");
Console.WriteLine($"20 / 4 = {calc.Divide(20, 4)}");

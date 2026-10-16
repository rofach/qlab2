namespace QLAB_2;

public class Calculator
{
    public double LastResult { get; private set; }
    public int ExecutionCount { get; private set; }
    public List<string> History { get; } = new();

    public double Add(double a, double b)
    {
        double result = a + b;
        Record("Add", $"{a} + {b} = {result}", result);
        return result;
    }

    public double Subtract(double a, double b)
    {
        double result = a - b;
        Record("Subtract", $"{a} - {b} = {result}", result);
        return result;
    }

    public double Multiply(double a, double b)
    {
        double result = a * b;
        Record("Multiply", $"{a} * {b} = {result}", result);
        return result;
    }

    public double Divide(double a, double b)
    {
        if (b == 0.0 || Math.Abs(b) < 1e-15)
        {
            throw new DivideByZeroException("Ділення на нуль неможливе.");
        }

        double result = a / b;
        Record("Divide", $"{a} / {b} = {result}", result);
        return result;
    }

    public void Reset()
    {
        LastResult = 0.0;
        ExecutionCount = 0;
        History.Clear();
    }

    private void Record(string operation, string details, double result)
    {
        LastResult = result;
        ExecutionCount++;
        History.Add($"[{ExecutionCount}] {details}");
    }
}

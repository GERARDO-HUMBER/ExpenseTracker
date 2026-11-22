namespace MyBudget.Core;

// Creates validated
public static class ExpenseFactory
{
    // Maximum allowed amount for an expense
    private const decimal MaxAmount = 1_000_000m;

    // Validate and round an amount
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new InvalidExpenseException(
                "Amount must be greater than zero.");
        }

        if (amount > MaxAmount)
        {
            throw new InvalidExpenseException(
                $"Amount cannot exceed {MaxAmount}.");
        }

        return decimal.Round(amount, 2);
    }

    // Validate description
    private static string ValidateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException(
                "Description cannot be empty.");
        }

        return description.Trim();
    }

    // Create expense one time
    public static OneTimeExpense CreateOneTime(
        string description,
        decimal amount,
        ExpenseCategory category,
        DateOnly date)
    {
        string validDescription = ValidateDescription(description);
        decimal validAmount = ValidateAmount(amount);

        return new OneTimeExpense(
            Guid.NewGuid(),
            validDescription,
            validAmount,
            category,
            date);
    }

    // Create recurring
    public static RecurringExpense CreateRecurring(
        string description,
        decimal amount,
        ExpenseCategory category,
        DateOnly date,
        int timesPerMonth)
    {
        string validDescription = ValidateDescription(description);
        decimal validAmount = ValidateAmount(amount);

        if (timesPerMonth < 1)
        {
            throw new InvalidExpenseException(
                "Frequency must be at least one.");
        }

        return new RecurringExpense(
            Guid.NewGuid(),
            validDescription,
            validAmount,
            category,
            date,
            timesPerMonth);
    }
}
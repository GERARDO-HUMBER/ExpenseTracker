namespace MyBudget.Core;

// Manages the monthly budget and evaluates spending
public sealed class BudgetService : IBudgetService
{
    // Monthly limit
    public decimal MonthlyLimit { get; private set; }

    // Set and validate budget
    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0m)
        {
            throw new InvalidExpenseException(
                "Monthly budget must be greater than zero.");
        }

        MonthlyLimit = limit;
    }

    // Calculate the remaining budget
    public decimal Remaining(decimal totalSpent)
    {
        return MonthlyLimit - totalSpent;
    }

    // Evaluate the current budget status
    public BudgetStatus Evaluate(decimal totalSpent)
    {
        // No monthly budget has been configured
        if (MonthlyLimit <= 0m)
        {
            return BudgetStatus.NotSet;
        }

        decimal remaining = Remaining(totalSpent);

        // Spending exceeds the monthly budget
        if (remaining < 0m)
        {
            return BudgetStatus.OverBudget;
        }

        // Less than 10% of the budget remains
        if (remaining < MonthlyLimit * 0.10m)
        {
            return BudgetStatus.AlmostOut;
        }

        // At least 10% of the budget remains
        return BudgetStatus.OnTrack;
    }
}
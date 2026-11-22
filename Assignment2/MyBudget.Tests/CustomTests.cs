using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

// Custom Tests
public class CustomExpenseTests
{
    [Fact]
    public void CreateOneTime_TrimsDescriptionAndRoundsAmount()
    {
        // Arrange
        string description = "  Groceries  ";
        decimal amount = 25.999m;
        DateOnly date = new(2026, 11, 1);

        // Act
        OneTimeExpense expense = ExpenseFactory.CreateOneTime(
            description,
            amount,
            ExpenseCategory.Food,
            date);

        // Assert
        Assert.Equal("Groceries", expense.Description);
        Assert.Equal(26.00m, expense.Amount);
        Assert.Equal(26.00m, expense.MonthlyImpact);
    }

    [Theory]
    [InlineData(2, 10, 20)]
    [InlineData(5, 4, 20)]
    [InlineData(12, 3, 36)]
    public void RecurringExpense_CalculatesMonthlyImpact(
        int amount,
        int timesPerMonth,
        int expected)
    {
        // Arrange
        DateOnly date = new(2026, 11, 1);

        // Act
        RecurringExpense expense = ExpenseFactory.CreateRecurring(
            "Transport",
            amount,
            ExpenseCategory.Transport,
            date,
            timesPerMonth);

        // Assert
        Assert.Equal((decimal)expected, expense.MonthlyImpact);
    }

    [Fact]
    public void BudgetService_ExactlyTenPercentRemaining_IsOnTrack()
    {
        // Arrange
        BudgetService budget = new();
        budget.SetMonthlyLimit(1000m);

        // Act
        BudgetStatus status = budget.Evaluate(900m);

        // Assert
        Assert.Equal(BudgetStatus.OnTrack, status);
        Assert.Equal(100m, budget.Remaining(900m));
    }
}
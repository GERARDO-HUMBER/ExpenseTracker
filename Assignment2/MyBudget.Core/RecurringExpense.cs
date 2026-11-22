namespace MyBudget.Core;

// Expense
// Times per month
public sealed record RecurringExpense(
    Guid Id,
    string Description,
    decimal Amount,
    ExpenseCategory Category,
    DateOnly Date,
    int TimesPerMonth)
    : Expense(Id, Description, Amount, Category, Date)
{
    // Calculate total monthly
    public override decimal MonthlyImpact => Amount * TimesPerMonth;

    // Monthly frequency
    public override string ToReportLine()
    {
        return $"{base.ToReportLine()} (x{TimesPerMonth}/month)";
    }
}

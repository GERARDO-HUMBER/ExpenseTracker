using System.Text.Json.Serialization;

namespace MyBudget.Core;

// Base record for all expense types
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(OneTimeExpense), "onetime")]
[JsonDerivedType(typeof(RecurringExpense), "recurring")]
public abstract record Expense(
    Guid Id,
    string Description,
    decimal Amount,
    ExpenseCategory Category,
    DateOnly Date) : IReportable
{
    // Monthly cost
    public abstract decimal MonthlyImpact { get; }

    // Expense summary
    public virtual string ToReportLine()
    {
        return $"{Date:yyyy-MM-dd} | {Description} | {Category} | {Amount:0.00}";
    }
}

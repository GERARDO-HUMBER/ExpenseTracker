namespace MyBudget.Core;

// Expense only once
public sealed record OneTimeExpense(
    Guid Id,
    string Description,
    decimal Amount,
    ExpenseCategory Category,
    DateOnly Date)
    : Expense(Id, Description, Amount, Category, Date)
{
    //One time expense
    public override decimal MonthlyImpact => Amount;
}

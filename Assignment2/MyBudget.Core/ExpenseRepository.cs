namespace MyBudget.Core;
// Manages expenses in memory and delegates persistence to IExpenseStore
public sealed class ExpenseRepository : IExpenseRepository
{
    // Storage abstraction used by the repository
    private readonly IExpenseStore _store;

    // In-memory collection of expenses
    private readonly List<Expense> _expenses;

    // Load existing expenses when the repository is created
    public ExpenseRepository(IExpenseStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _expenses = _store.Load().ToList();
    }

    // Return all registered expenses
    public IReadOnlyList<Expense> GetAll()
    {
        return _expenses.AsReadOnly();
    }

    // Add a new expense to the collection
    public void Add(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        _expenses.Add(expense);
    }

    // Calculate the total monthly impact of all expenses
    public decimal Total()
    {
        return _expenses.Sum(expense => expense.MonthlyImpact);
    }

    // Group expenses by category and calculate their totals
    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
    {
        return _expenses
            .GroupBy(expense => expense.Category)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(expense => expense.MonthlyImpact));
    }

    // Return expenses belonging to the selected category
    public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
    {
        return _expenses
            .Where(expense => expense.Category == category)
            .ToList();
    }

    // Save all expenses through the storage abstraction
    public void Save()
    {
        _store.Save(_expenses);
    }
}
// =====================================================================
//  Program.cs  —  the interactive console UI for MyBudget (Assignment 1).
//  Target framework: .NET 10 (LTS), language C# 14.
//
//  >>> BUILD THE MENU-DRIVEN UI HERE (Modules 1-3). <<<
//
//  Once you have implemented BudgetRules.cs (so the unit tests pass), wire it
//  up to a console interface that meets the assignment brief:
//
//    * Print a banner (try a raw string literal).
//    * Loop a menu until the user exits, using a switch on the choice:
//        1) Add an expense   2) View summary   3) Set monthly budget   4) Exit
//    * Read and VALIDATE input, re-prompting on bad data (decimal.TryParse,
//      BudgetRules.NormalizeCategory, a date parse, non-empty text).
//    * Keep running totals in simple variables (no collections / no classes).
//    * Use BudgetRules.ValidateAmount / ClassifyAmount / BudgetStatus /
//      FormatCurrency for all logic and formatting.
//    * Handle bad input with try / catch / finally and InvalidExpenseException.
//
//  See section 6 of the assignment brief for a sample run to aim for.
// =====================================================================
using ExpenseTracker;

//Console.WriteLine("MyBudget — TODO: build the interactive menu UI (see the brief).");

// Delete the line above and implement the application here.

// Banner
Console.WriteLine("""
    ====================================
          MYBUDGET EXPENSE TRACKER
    ====================================
    """);

bool running = true;

// Main
while (running)
{
    Console.WriteLine("""

        1. Add an expense
        2. View summary
        3. Set monthly budget
        4. Exit
        """);

    Console.Write("Select an option: ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddExpense();
            break;

        case "2":
            Console.WriteLine("View summary");
            break;

        case "3":
            Console.WriteLine("Set monthly budget");
            break;

        case "4":
            running = false;
            break;

        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

// Feature 1: Add an expense
static void AddExpense()
{
    Console.WriteLine("\n--- Add an expense ---");

    string? description;

    do
    {
        Console.Write("Enter expense description: ");
        description = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Description cannot be empty.");
        }

    } while (string.IsNullOrWhiteSpace(description));

    description = description.Trim();

    Console.WriteLine($"Description: {description}");
}

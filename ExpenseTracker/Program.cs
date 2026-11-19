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

using System.Globalization;

// Banner
Console.WriteLine("""
    ====================================
          MYBUDGET EXPENSE TRACKER
    ====================================
    """);

bool running = true;

//accumulators
int expenseCount = 0;
decimal totalExpenses = 0m;
decimal highestExpense = 0m;

//totals by category
decimal foodTotal = 0m;
decimal transportTotal = 0m;
decimal utilitiesTotal = 0m;
decimal entertainmentTotal = 0m;
decimal otherTotal = 0m;

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
void AddExpense()
{
    Console.WriteLine("\n--- Add an expense ---");

    try
    {
        
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

        // Read and validate the expense amount
        decimal amount;

        while (true)
        {
            Console.Write("Enter expense amount: ");
            string? amountInput = Console.ReadLine();

            if (!decimal.TryParse(amountInput, out amount))
            {
                Console.WriteLine(
                    "Invalid amount. Enter a valid number.");
                continue;
            }

            try
            {
                amount = BudgetRules.ValidateAmount(amount);

                if (amount <= 0m)
                {
                    throw new InvalidExpenseException(
                        "Rounded amount must be greater than zero.");
                }

                break;
            }
            catch (InvalidExpenseException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Classify the expense amount
        string classification = BudgetRules.ClassifyAmount(amount);

        // Read and validate the expense category
        string? category;

        do
        {
            Console.WriteLine(
                "Categories: Food, Transport, Utilities, Entertainment, Other");

            Console.Write("Enter expense category: ");

            category = BudgetRules.NormalizeCategory(Console.ReadLine());

            if (category is null)
            {
                Console.WriteLine(
                    "Invalid category. Please try again.");
            }

        } while (category is null);

        // Read and validate the expense date
        DateTime expenseDate;

        while (true)
        {
            Console.Write(
                "Enter date (yyyy-MM-dd, blank for today): ");

            string? dateInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(dateInput))
            {
                expenseDate = DateTime.Today;
                break;
            }

            if (!DateTime.TryParseExact(
                    dateInput.Trim(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out expenseDate))
            {
                Console.WriteLine(
                    "Invalid date. Use yyyy-MM-dd format.");
                continue;
            }

            if (expenseDate > DateTime.Today)
            {
                Console.WriteLine(
                    "Expense date cannot be in the future.");
                continue;
            }

            break;
        }

        // Read the optional expense note
        Console.Write("Enter an optional note: ");
        string? note = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(note))
        {
            note = null;
        }

        // Update expense accumulators
        expenseCount++;
        totalExpenses += amount;

        // Update highest expense
        if (amount > highestExpense)
        {
            highestExpense = amount;
        }

        // Update category total
        switch (category)
        {
            case "Food":
                foodTotal += amount;
                break;

            case "Transport":
                transportTotal += amount;
                break;

            case "Utilities":
                utilitiesTotal += amount;
                break;

            case "Entertainment":
                entertainmentTotal += amount;
                break;

            case "Other":
                otherTotal += amount;
                break;
        }

        // Display the registered expense
        Console.WriteLine("\n--- Expense details ---");
        Console.WriteLine($"Description: {description}");
        Console.WriteLine($"Amount: {BudgetRules.FormatCurrency(amount)}");
        Console.WriteLine($"Classification: {classification}");
        Console.WriteLine($"Category: {category}");
        Console.WriteLine($"Date: {expenseDate:yyyy-MM-dd}");
        Console.WriteLine($"Note: {note ?? "No note provided."}");

        Console.WriteLine("\nExpense added successfully!");

        // Show current running totals
        Console.WriteLine($"Expenses recorded: {expenseCount}");
        Console.WriteLine(
            $"Running total: {BudgetRules.FormatCurrency(totalExpenses)}");
    }
    catch (InvalidExpenseException ex)
    {
        Console.WriteLine($"Expense registration failed: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("\nReturning to the main menu...");
    }
}
using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

// Stores and retrieves expenses using a JSON file
public sealed class JsonExpenseStore : IExpenseStore
{
    // Path of the JSON file
    private readonly string _filePath;

    // JSON serialization settings
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    // Receive the file path through the constructor
    public JsonExpenseStore(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "File path cannot be empty.",
                nameof(filePath));
        }

        _filePath = filePath;
    }

    // Load expenses from the JSON file
    public IReadOnlyList<Expense> Load()
    {
        // Return an empty collection when the file does not exist
        if (!File.Exists(_filePath))
        {
            return new List<Expense>();
        }

        // Read the JSON content
        string json = File.ReadAllText(_filePath);

        // Return an empty collection when the file is blank
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Expense>();
        }

        // Convert JSON into expense objects
        List<Expense>? expenses =
            JsonSerializer.Deserialize<List<Expense>>(
                json,
                JsonOptions);

        return expenses ?? new List<Expense>();
    }

    // Save expenses to the JSON file
    public void Save(IEnumerable<Expense> expenses)
    {
        ArgumentNullException.ThrowIfNull(expenses);

        // Create the destination directory if necessary
        string? directory = Path.GetDirectoryName(
            Path.GetFullPath(_filePath));

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        // Convert expense objects into JSON
        string json = JsonSerializer.Serialize(
            expenses.ToList(),
            JsonOptions);

        // Write the JSON content to the file
        File.WriteAllText(_filePath, json);
    }
}
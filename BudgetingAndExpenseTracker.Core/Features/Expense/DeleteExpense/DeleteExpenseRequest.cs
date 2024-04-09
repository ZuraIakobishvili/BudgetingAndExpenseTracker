using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;

public class DeleteExpenseRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = String.Empty;
    public string? ExpenseId { get; set; }
}

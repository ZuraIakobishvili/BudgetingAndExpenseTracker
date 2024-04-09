using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.GetExpenses;

public class GetAllExpensesRequest
{
    [JsonIgnore]
    public string? UserId { get; set; } = string.Empty;
}

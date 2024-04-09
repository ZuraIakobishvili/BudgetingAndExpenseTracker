using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;
public class UpdateExpenseRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public Guid ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public ExpenseCategory Category { get; set; }
}

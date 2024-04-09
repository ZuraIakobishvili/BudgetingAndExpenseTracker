using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;
public class CreateExpenseRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public ExpenseCategory Category { get; set; }
}


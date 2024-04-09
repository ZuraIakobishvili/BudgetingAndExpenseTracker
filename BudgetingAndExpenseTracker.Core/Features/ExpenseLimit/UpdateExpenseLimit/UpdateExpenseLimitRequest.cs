using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;
public class UpdateExpenseLimitRequest
{
    public string? Id { get; set; }
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public ExpenseCategory Category { get; set; }
    public Currency Currency { get; set; }
    public Period Period { get; set; }

}

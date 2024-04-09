using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
public class CreateExpenseLimitRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public Currency Currency { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public Period LimitPeriod { get; set; }
}

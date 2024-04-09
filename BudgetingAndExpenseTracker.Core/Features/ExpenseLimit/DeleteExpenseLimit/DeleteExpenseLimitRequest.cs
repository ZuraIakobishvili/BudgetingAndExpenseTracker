using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.DeleteExpenseLimit;

public class DeleteExpenseLimitRequest
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public string? LimitId { get; set; }

}

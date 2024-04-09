using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.GetExpenseLimits;

public class GetAllExpenseLimitsRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}

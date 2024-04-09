using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Income.GetIncomes;
public class GetAllIncomesRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}

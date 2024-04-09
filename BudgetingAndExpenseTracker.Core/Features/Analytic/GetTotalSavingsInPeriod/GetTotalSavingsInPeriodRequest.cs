using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;

public class GetTotalSavingsInPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public Period Period { get; set; }
    public Currency Currency { get; set; }
}

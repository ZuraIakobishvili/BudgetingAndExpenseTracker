using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;
public class GetIncomesByCategoryAndPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public IncomeCategory Category { get; set; }
    public Period Period { get; set; }
}

using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;
public class GetExpensesByCurrencyAndPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public Currency Currency{ get; set; }
    public Period Period { get; set; }
}

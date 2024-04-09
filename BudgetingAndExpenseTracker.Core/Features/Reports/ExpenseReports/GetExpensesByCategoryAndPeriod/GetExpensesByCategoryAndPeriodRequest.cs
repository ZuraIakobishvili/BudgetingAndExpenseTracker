using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;

public class GetExpensesByCategoryAndPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public ExpenseCategory Category { get; set; }
    public Period Period { get; set; }
}

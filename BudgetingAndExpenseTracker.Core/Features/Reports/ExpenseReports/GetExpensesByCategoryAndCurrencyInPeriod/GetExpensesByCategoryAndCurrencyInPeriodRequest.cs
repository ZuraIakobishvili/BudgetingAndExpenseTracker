using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;
public class GetExpensesByCategoryAndCurrencyInPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public ExpenseCategory Category {  get; set; }
    public Currency Currency { get; set; }
    public Period Period { get; set; }
}

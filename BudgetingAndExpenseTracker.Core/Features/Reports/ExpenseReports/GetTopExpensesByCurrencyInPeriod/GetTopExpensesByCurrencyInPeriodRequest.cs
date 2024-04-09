using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;
public class GetTopExpensesByCurrencyInPeriodRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public int TopExpensesCount { get; set; }
    public Currency Currency { get; set; }
    public Period Period { get; set; }
}

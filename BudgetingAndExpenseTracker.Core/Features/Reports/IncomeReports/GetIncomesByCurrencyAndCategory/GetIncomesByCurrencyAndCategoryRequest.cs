using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
public class GetIncomesByCurrencyAndCategoryRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = String.Empty;
    public Currency Currency { get; set; }
    public IncomeCategory Category { get; set; }
    public Period Period { get; set; }
}

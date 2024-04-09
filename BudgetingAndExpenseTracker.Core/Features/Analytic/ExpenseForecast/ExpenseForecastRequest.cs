using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;

public class ExpenseForecastRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public ExpenseCategory Category { get; set; }
    public Currency Currency { get; set; }
}
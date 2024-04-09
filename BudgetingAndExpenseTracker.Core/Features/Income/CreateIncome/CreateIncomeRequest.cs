using BudgetingAndExpenseTracker.Core.Shared;
using NSwag.Annotations;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
public class CreateIncomeRequest
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public IncomeCategory Category { get; set; }
}

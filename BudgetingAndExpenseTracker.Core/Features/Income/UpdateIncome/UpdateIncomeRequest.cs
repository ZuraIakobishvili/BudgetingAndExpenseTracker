using BudgetingAndExpenseTracker.Core.Shared;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;
public class UpdateIncomeRequest
{
    public  string? Id { get; set; }
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public IncomeCategory Category { get; set; }
}

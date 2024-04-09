using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;

public class DeleteIncomeRequest
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public string? IncomeId { get; set; }
}

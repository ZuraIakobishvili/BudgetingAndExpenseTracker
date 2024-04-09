using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetSavingsPercentInPeriod;

public class GetSavingsPercentInPeriodResponse
{
    public string? Message { get; set; }
    public decimal Percent { get; set; }
    public Currency Currency { get; set; }
    public Period Period { get; set; }

}

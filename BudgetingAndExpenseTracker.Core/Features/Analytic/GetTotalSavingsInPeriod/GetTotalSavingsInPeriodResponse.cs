using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;
public class GetTotalSavingsInPeriodResponse
{
    public string? Message { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public Period Period { get; set; }

}

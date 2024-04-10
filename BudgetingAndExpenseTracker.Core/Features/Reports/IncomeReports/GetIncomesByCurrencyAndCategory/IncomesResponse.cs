using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;

public class IncomesResponse
{
    public Guid IncomeId { get; set; }
    public decimal Amount { get; set; }
    public IncomeCategory Category { get; set; }
    public Currency Currency { get; set; }
    public DateTime IncomeDate { get; set; }
}

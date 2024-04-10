using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;

public class ExpensesResponse
{
    public Guid ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public Currency Currency { get; set; }
    public DateTime ExpenseDate { get; set; }
}

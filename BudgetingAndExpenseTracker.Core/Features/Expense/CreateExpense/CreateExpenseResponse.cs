using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;

public class CreateExpenseResponse 
{
    public string? Message { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public ExpenseCategory Category { get; set; }
}

using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public  AppUser? User { get; set; }
    public Currency Currency { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
}

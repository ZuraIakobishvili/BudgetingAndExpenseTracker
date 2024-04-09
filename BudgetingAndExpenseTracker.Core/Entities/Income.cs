using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Entities;
public class Income
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public  AppUser? User { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public IncomeCategory Category { get; set; }
    public DateTime IncomeDate { get; set; }
}

using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
public class CreateExpenseLimitResponse : CreateExpenseLimitRequest
{
    public string? Message { get; set; }
}

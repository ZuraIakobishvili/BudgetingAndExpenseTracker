using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
public class CreateIncomeResponse : CreateIncomeRequest
{
    public required string Message { get; set; }
}

using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;

public class UpdateExpenseValidation
{
    public static bool IsPeriodExpenseLimitExceeded(UpdateExpenseRequest request, Entities.ExpenseLimit limitPeriod, Dictionary<(ExpenseCategory, Currency), decimal> periodExpenses)
    {
        return limitPeriod != null
        && periodExpenses.TryGetValue((request.Category, request.Currency), out var existingExpense)
        && existingExpense + request.Amount > limitPeriod.Amount;
    }
}

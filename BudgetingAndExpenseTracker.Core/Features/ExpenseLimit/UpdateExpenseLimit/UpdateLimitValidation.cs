using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
using BudgetingAndExpenseTracker.Core.Shared;
namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;

public class UpdateLimitValidation
{
    public static void LimitValidation(UpdateExpenseLimitRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Update expense limit request cannot be null.");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidRequestException("Limit amount can not be zero or negative, try again.");
        }

        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Limit category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Limit currency is not valid.");
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Limit period is not valid.");
        }
    }
}

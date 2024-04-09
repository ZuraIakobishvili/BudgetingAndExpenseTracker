using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;

public  class CreateIncomeValidation
{
    public static void IncomeValidation(CreateIncomeRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new InvalidRequestException("Income amount can not be zero or negative, try again.");
        }

        if (!Enum.IsDefined(typeof(IncomeCategory), request.Category))
        {
            throw new InvalidRequestException("Invalid income category.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Invalid income currency.");
        }
    }
}

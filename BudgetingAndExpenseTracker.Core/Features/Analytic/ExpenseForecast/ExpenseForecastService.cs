using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;

public interface IExpenseForecastService
{
    Task<decimal> GetNextMonthExpenseForecastAsync(ExpenseForecastRequest request);

}
public class ExpenseForecastService : IExpenseForecastService
{
    private readonly IExpenseForecastRepository _expenseForecastRepository;
    public ExpenseForecastService(IExpenseForecastRepository expenseForecastRepository)
    {
        _expenseForecastRepository = expenseForecastRepository;
    }

    public async Task<decimal> GetNextMonthExpenseForecastAsync(ExpenseForecastRequest request)
    {
        ValidateForecastRequest(request);
        return await _expenseForecastRepository.GetNextMonthExpenseForecastAsync(request);
    }

    private void ValidateForecastRequest(ExpenseForecastRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Limit category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Limit currency is not valid.");
        }
    }
}

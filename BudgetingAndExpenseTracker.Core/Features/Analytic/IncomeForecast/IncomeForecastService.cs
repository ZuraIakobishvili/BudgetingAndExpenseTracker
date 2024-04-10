using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;

public interface IIncomeForecastService
{
    Task<decimal> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request);
}
public class IncomeForecastService : IIncomeForecastService
{
    private readonly IIncomeForecastRepository _incomeForecastRepository;
    public IncomeForecastService(IIncomeForecastRepository incomeForecastRepository)
    {
        _incomeForecastRepository = incomeForecastRepository;
    }

    public async Task<decimal> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request)
    {
        ValidateIncomeForecastRequest(request);
        return await _incomeForecastRepository.GetNextMonthIncomeForecastAsync(request);
    }

    private void ValidateIncomeForecastRequest(IncomeForecastRequest request)
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

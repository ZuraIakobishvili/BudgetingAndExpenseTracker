using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;

public interface IIncomeForecastService
{
    Task<IncomeForecastResponse> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request);
}
public class IncomeForecastService : IIncomeForecastService
{
    private readonly IIncomeForecastRepository _incomeForecastRepository;
    public IncomeForecastService(IIncomeForecastRepository incomeForecastRepository)
    {
        _incomeForecastRepository = incomeForecastRepository;
    }

    public async Task<IncomeForecastResponse> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request)
    {
        ValidateIncomeForecastRequest(request);
        var incomeForecast =  await _incomeForecastRepository.GetNextMonthIncomeForecastAsync(request);

        return new IncomeForecastResponse
        {
            Message = $"Forecast for the next month in category-{request.Category} and currency-{request.Currency}: ",
            Amount = incomeForecast
        };
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

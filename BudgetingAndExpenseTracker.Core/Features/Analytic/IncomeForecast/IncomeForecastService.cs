using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;

public interface IIncomeForecastService
{
    Task<Dictionary<string, decimal>> GetNextMonthIncomeForecast(IncomeForecastRequest request);
}
public class IncomeForecastService : IIncomeForecastService
{
    private readonly IIncomeForecastRepository _incomeForecastRepository;
    public IncomeForecastService(IIncomeForecastRepository incomeForecastRepository)
    {
        _incomeForecastRepository = incomeForecastRepository;
    }

    public async Task<Dictionary<string, decimal>> GetNextMonthIncomeForecast(IncomeForecastRequest request)
    {
        var forecast = await _incomeForecastRepository.GetNextMonthIncomeForecast(request);
        if (forecast == null)
        {
            throw new InvalidForecastException("Forecast can not be calculated.");
        }

        return forecast;
    }
}

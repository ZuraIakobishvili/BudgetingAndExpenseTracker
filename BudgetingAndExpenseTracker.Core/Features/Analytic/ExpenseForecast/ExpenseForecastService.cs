using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;

public interface IExpenseForecastService
{
    Task<Dictionary<string, decimal>> GetNextMonthExpenseForecast(ExpenseForecastRequest request);

}
public class ExpenseForecastService : IExpenseForecastService
{
    private readonly IExpenseForecastRepository _expenseForecastRepository;
    public ExpenseForecastService(IExpenseForecastRepository expenseForecastRepository)
    {
        _expenseForecastRepository = expenseForecastRepository;
    }

    public async Task<Dictionary<string, decimal>> GetNextMonthExpenseForecast(ExpenseForecastRequest request)
    {
        var forecast =  await _expenseForecastRepository.GetNextMonthExpenseForecast(request);
        if (forecast == null)
        {
            throw new InvalidForecastException("Forecast can not be calculated.");
        }

        return forecast;
    }
}

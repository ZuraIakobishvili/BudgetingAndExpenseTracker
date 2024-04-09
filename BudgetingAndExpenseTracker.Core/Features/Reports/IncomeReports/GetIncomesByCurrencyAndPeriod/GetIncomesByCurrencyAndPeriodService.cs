using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;

public interface IGetIncomesByCurrencyAndPeriodService
{
    Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriod(GetIncomesByCurrencyAndPeriodRequest request);
}

public class GetIncomesByCurrencyAndPeriodService : IGetIncomesByCurrencyAndPeriodService
{
    private readonly IGetIncomesByCurrencyAndPeriodRepository _getIncomesByCurrencyAndPeriodRepository;
    public GetIncomesByCurrencyAndPeriodService(IGetIncomesByCurrencyAndPeriodRepository getIncomesByCurrencyAndPeriodRepository)
    {
        _getIncomesByCurrencyAndPeriodRepository = getIncomesByCurrencyAndPeriodRepository;
    }

    public async Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriod(GetIncomesByCurrencyAndPeriodRequest request)
    {
        var incomes = await _getIncomesByCurrencyAndPeriodRepository.GetIncomesByCurrencyAndPeriod(request);
        if (!incomes.Any())
        {
            throw new InvalidIncomeException("Incomes by category not found in this period.");
        }

        return incomes;
    }
}

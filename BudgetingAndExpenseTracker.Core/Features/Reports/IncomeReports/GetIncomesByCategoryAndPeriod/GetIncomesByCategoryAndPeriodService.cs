using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodService
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndPeriod(GetIncomesByCategoryAndPeriodRequest request);
}
public class GetIncomesByCategoryAndPeriodService : IGetIncomesByCategoryAndPeriodService
{
    private readonly IGetIncomesByCategoryAndPeriodRepository _getIncomesByCategoryAndPeriodRepository;
    public GetIncomesByCategoryAndPeriodService(IGetIncomesByCategoryAndPeriodRepository getIncomesByCategoryAndPeriodRepository)
    {
        _getIncomesByCategoryAndPeriodRepository = getIncomesByCategoryAndPeriodRepository;
    }
    public async Task<List<Entities.Income>> GetIncomesByCategoryAndPeriod(GetIncomesByCategoryAndPeriodRequest request)
    {
        var incomes = await _getIncomesByCategoryAndPeriodRepository.GetIncomesByCategoryAndPeriod(request);
        if (!incomes.Any()) 
        {
            throw new InvalidIncomeException("Incomes not found in specific category and period");
        }

        return incomes;
    }
}


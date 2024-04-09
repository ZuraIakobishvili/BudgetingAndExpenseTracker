using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;

public interface IGetIncomesByCurrencyAndCategoryService
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndCurrencyInPeriod(GetIncomesByCurrencyAndCategoryRequest request);
}
public class GetIncomesByCurrencyAndCategoryService : IGetIncomesByCurrencyAndCategoryService
{
    private readonly IGetIncomesByCurrencyAndCategoryRepository _getIncomesByCurrencyAndCategoryRepository;
    public GetIncomesByCurrencyAndCategoryService(IGetIncomesByCurrencyAndCategoryRepository getIncomesByCurrencyAndCategoryRepository)
    {
        _getIncomesByCurrencyAndCategoryRepository = getIncomesByCurrencyAndCategoryRepository;
    }

    public async Task<List<Entities.Income>> GetIncomesByCategoryAndCurrencyInPeriod(GetIncomesByCurrencyAndCategoryRequest request)
    {
        var incomes = await _getIncomesByCurrencyAndCategoryRepository.GetIncomesByCurrencyAndCategory(request);
        if (!incomes.Any())
        {
            throw new InvalidIncomeException("Incomes are not found in specific currenct and category");
        }

        return incomes;
    }
}


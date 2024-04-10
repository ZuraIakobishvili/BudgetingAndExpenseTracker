using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;

public interface IGetIncomesByCurrencyAndCategoryService
{
    Task<List<IncomesResponse>> GetIncomesByCategoryAndCurrencyInPeriodAsync(GetIncomesByCurrencyAndCategoryRequest request);
}
public class GetIncomesByCurrencyAndCategoryService : IGetIncomesByCurrencyAndCategoryService
{
    private readonly IGetIncomesByCurrencyAndCategoryRepository _getIncomesByCurrencyAndCategoryRepository;
    public GetIncomesByCurrencyAndCategoryService(IGetIncomesByCurrencyAndCategoryRepository getIncomesByCurrencyAndCategoryRepository)
    {
        _getIncomesByCurrencyAndCategoryRepository = getIncomesByCurrencyAndCategoryRepository;
    }

    public async Task<List<IncomesResponse>> GetIncomesByCategoryAndCurrencyInPeriodAsync(GetIncomesByCurrencyAndCategoryRequest request)
    {
        ValidateIncomesRequest(request);
        var incomes = await _getIncomesByCurrencyAndCategoryRepository.GetIncomesByCategoryAndCurrencyInPeriodAsync(request);
        if (!incomes.Any())
        {
            throw new InvalidIncomeException("Incomes are not found in specific currenct and category");
        }

        var incomesResponse = incomes.Select(i => new IncomesResponse
        {
            IncomeId = i.Id,
            Amount = i.Amount,
            Category = i.Category,
            Currency = i.Currency,
            IncomeDate = i.IncomeDate
        }).ToList();

        return incomesResponse;
    }

    private void ValidateIncomesRequest(GetIncomesByCurrencyAndCategoryRequest request)
    {
        if (request == null)
        {
            throw new ArgumentException(nameof(request));
        }

        if (!Enum.IsDefined(typeof(IncomeCategory), request.Category))
        {
            throw new InvalidRequestException("Income category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Income period is not valid.");
        }
        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Income currency is not valid.");
        }
    }
}


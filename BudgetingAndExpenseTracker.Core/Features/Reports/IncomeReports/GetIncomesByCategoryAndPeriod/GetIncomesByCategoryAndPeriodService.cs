using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodService
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request);
}
public class GetIncomesByCategoryAndPeriodService : IGetIncomesByCategoryAndPeriodService
{
    private readonly IGetIncomesByCategoryAndPeriodRepository _getIncomesByCategoryAndPeriodRepository;
    public GetIncomesByCategoryAndPeriodService(IGetIncomesByCategoryAndPeriodRepository getIncomesByCategoryAndPeriodRepository)
    {
        _getIncomesByCategoryAndPeriodRepository = getIncomesByCategoryAndPeriodRepository;
    }
    public async Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request)
    {
        ValidateIncomesRequest(request);

        var incomes = await _getIncomesByCategoryAndPeriodRepository.GetIncomesByCategoryAndPeriodAsync(request);
        if (!incomes.Any()) 
        {
            throw new InvalidIncomeException("Incomes not found in specific category and period");
        }

        return incomes;
    }

    private void ValidateIncomesRequest(GetIncomesByCategoryAndPeriodRequest request)
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
    }
}


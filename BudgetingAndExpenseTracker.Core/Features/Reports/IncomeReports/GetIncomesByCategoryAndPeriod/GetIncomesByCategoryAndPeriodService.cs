using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodService
{
    Task<List<IncomesResponse>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request);
}
public class GetIncomesByCategoryAndPeriodService : IGetIncomesByCategoryAndPeriodService
{
    private readonly IGetIncomesByCategoryAndPeriodRepository _getIncomesByCategoryAndPeriodRepository;
    public GetIncomesByCategoryAndPeriodService(IGetIncomesByCategoryAndPeriodRepository getIncomesByCategoryAndPeriodRepository)
    {
        _getIncomesByCategoryAndPeriodRepository = getIncomesByCategoryAndPeriodRepository;
    }
    public async Task<List<IncomesResponse>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request)
    {
        ValidateIncomesRequest(request);

        var incomes = await _getIncomesByCategoryAndPeriodRepository.GetIncomesByCategoryAndPeriodAsync(request);
        if (!incomes.Any())
        {
            throw new InvalidIncomeException("Incomes not found in specific category and period");
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


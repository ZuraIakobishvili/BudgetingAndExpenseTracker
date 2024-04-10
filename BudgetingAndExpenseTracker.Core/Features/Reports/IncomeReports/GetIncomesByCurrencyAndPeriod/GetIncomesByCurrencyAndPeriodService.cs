using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;

public interface IGetIncomesByCurrencyAndPeriodService
{
    Task<List<IncomesReposne>> GetIncomesByCurrencyAndPeriodAsync(GetIncomesByCurrencyAndPeriodRequest request);
}

public class GetIncomesByCurrencyAndPeriodService : IGetIncomesByCurrencyAndPeriodService
{
    private readonly IGetIncomesByCurrencyAndPeriodRepository _getIncomesByCurrencyAndPeriodRepository;
    public GetIncomesByCurrencyAndPeriodService(IGetIncomesByCurrencyAndPeriodRepository getIncomesByCurrencyAndPeriodRepository)
    {
        _getIncomesByCurrencyAndPeriodRepository = getIncomesByCurrencyAndPeriodRepository;
    }

    public async Task<List<IncomesReposne>> GetIncomesByCurrencyAndPeriodAsync(GetIncomesByCurrencyAndPeriodRequest request)
    {
        ValidateIncomesRequest(request);

        var incomes = await _getIncomesByCurrencyAndPeriodRepository.GetIncomesByCurrencyAndPeriodAsync(request);
        if (!incomes.Any())
        {
            throw new InvalidIncomeException("Incomes by category not found in this period.");
        }

        var incomesResponse = incomes.Select(i => new IncomesReposne
        {
            IncomeId = i.Id,
            Amount = i.Amount,
            Category = i.Category,
            Currency = i.Currency,
            IncomeDate = i.IncomeDate
        }).ToList();

        return incomesResponse;
    }

    private void ValidateIncomesRequest(GetIncomesByCurrencyAndPeriodRequest request)
    {
        if (request == null)
        {
            throw new ArgumentException(nameof(request));
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Income currency is not valid.");
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Income period is not valid.");
        }
    }
}

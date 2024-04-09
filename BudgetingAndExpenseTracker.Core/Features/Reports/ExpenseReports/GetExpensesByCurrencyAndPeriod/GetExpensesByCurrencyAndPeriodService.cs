using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;

public interface IGetExpensesByCurrencyAndPeriodService
{
    Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriod(GetExpensesByCurrencyAndPeriodRequest request);
}
public class GetExpensesByCurrencyAndPeriodService : IGetExpensesByCurrencyAndPeriodService
{
    private readonly IGetExpensesByCurrencyAndPeriodRepository _getExpensesByCurrencyAndPeriodRepository;
    public GetExpensesByCurrencyAndPeriodService(IGetExpensesByCurrencyAndPeriodRepository getExpensesByCurrencyAndPeriodRepository)
    {
        _getExpensesByCurrencyAndPeriodRepository = getExpensesByCurrencyAndPeriodRepository;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriod(GetExpensesByCurrencyAndPeriodRequest request)
    {
        var expenses = await _getExpensesByCurrencyAndPeriodRepository.GetExpensesByCurrencyAndPeriod(request);

        if (!expenses.Any())
        {
            throw new InvalidExpenseException("No expenses found in the specified period and category.");
        }

        return expenses;
    }
}

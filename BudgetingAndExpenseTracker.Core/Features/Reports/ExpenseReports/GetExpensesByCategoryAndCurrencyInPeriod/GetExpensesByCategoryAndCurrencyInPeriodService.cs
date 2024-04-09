using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;

public interface IGetExpensesByCategoryAndCurrencyInPeriodService
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriod(GetExpensesByCategoryAndCurrencyInPeriodRequest request);
}
public class GetExpensesByCategoryAndCurrencyInPeriodService : IGetExpensesByCategoryAndCurrencyInPeriodService
{
    private readonly IGetExpensesByCategoryAndCurrencyInPeriodRepository _getExpensesByCategoryAndCurrencyInPeriodRepository;
    public GetExpensesByCategoryAndCurrencyInPeriodService(IGetExpensesByCategoryAndCurrencyInPeriodRepository getExpensesByCategoryAndCurrencyInPeriodRepository)
    {
        _getExpensesByCategoryAndCurrencyInPeriodRepository = getExpensesByCategoryAndCurrencyInPeriodRepository;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriod(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        var expenses = await _getExpensesByCategoryAndCurrencyInPeriodRepository.GetExpensesByCategoryAndCurrencyInPeriod(request);
        if (!expenses.Any())
        {
            throw new InvalidExpenseException("Expenses by category and currensy are not found in this period");
        }
        return expenses;
    }
}

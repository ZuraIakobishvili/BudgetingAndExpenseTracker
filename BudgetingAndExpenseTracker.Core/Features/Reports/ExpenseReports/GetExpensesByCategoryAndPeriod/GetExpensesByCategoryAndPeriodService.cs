using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;

public interface IGetExpensesByCategoryAndPeriodService
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriod(GetExpensesByCategoryAndPeriodRequest request);
}
public class GetExpensesByCategoryAndPeriodService : IGetExpensesByCategoryAndPeriodService
{
    private readonly IGetExpensesByCategoryAndPeriodRepository _getExpensesByCategoryAndPeriodRepository;
    public GetExpensesByCategoryAndPeriodService(IGetExpensesByCategoryAndPeriodRepository getExpensesByCategoryAndPeriodRepository)
    {
        _getExpensesByCategoryAndPeriodRepository = getExpensesByCategoryAndPeriodRepository;
    }
    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriod(GetExpensesByCategoryAndPeriodRequest request)
    {
        var expenses = await _getExpensesByCategoryAndPeriodRepository.GetExpensesByCategoryAndPeriod(request);

        if (!expenses.Any())
        {
            throw new InvalidExpenseException("No expenses found in the specified period and category.");
        }
        return expenses;
    }
}

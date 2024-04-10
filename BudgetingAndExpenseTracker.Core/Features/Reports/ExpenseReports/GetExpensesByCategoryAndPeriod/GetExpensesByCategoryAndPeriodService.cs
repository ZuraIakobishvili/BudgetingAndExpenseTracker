using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;

public interface IGetExpensesByCategoryAndPeriodService
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriodAsync(GetExpensesByCategoryAndPeriodRequest request);
}
public class GetExpensesByCategoryAndPeriodService : IGetExpensesByCategoryAndPeriodService
{
    private readonly IGetExpensesByCategoryAndPeriodRepository _getExpensesByCategoryAndPeriodRepository;
    public GetExpensesByCategoryAndPeriodService(IGetExpensesByCategoryAndPeriodRepository getExpensesByCategoryAndPeriodRepository)
    {
        _getExpensesByCategoryAndPeriodRepository = getExpensesByCategoryAndPeriodRepository;
    }
    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriodAsync(GetExpensesByCategoryAndPeriodRequest request)
    {
        var expenses = await _getExpensesByCategoryAndPeriodRepository.GetExpensesByCategoryAndPeriodAsync(request);

        if (!expenses.Any())
        {
            throw new InvalidExpenseException("No expenses found in the specified period and category.");
        }
        return expenses;
    }

    private void ValidateExpensesRequest(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        if (request == null)
        {
            throw new ArgumentException(nameof(request));
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Expense category is not valid.");
        }

        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Expense category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Expense category is not valid.");
        }
    }
}

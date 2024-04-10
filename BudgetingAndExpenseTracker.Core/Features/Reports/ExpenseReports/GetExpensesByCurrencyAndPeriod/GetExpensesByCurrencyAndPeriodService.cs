using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;

public interface IGetExpensesByCurrencyAndPeriodService
{
    Task<List<ExpensesResponse>> GetExpensesByCurrencyAndPeriodAsync(GetExpensesByCurrencyAndPeriodRequest request);
}
public class GetExpensesByCurrencyAndPeriodService : IGetExpensesByCurrencyAndPeriodService
{
    private readonly IGetExpensesByCurrencyAndPeriodRepository _getExpensesByCurrencyAndPeriodRepository;
    public GetExpensesByCurrencyAndPeriodService(IGetExpensesByCurrencyAndPeriodRepository getExpensesByCurrencyAndPeriodRepository)
    {
        _getExpensesByCurrencyAndPeriodRepository = getExpensesByCurrencyAndPeriodRepository;
    }

    public async Task<List<ExpensesResponse>> GetExpensesByCurrencyAndPeriodAsync(GetExpensesByCurrencyAndPeriodRequest request)
    {
        ValidateExpensesRequest(request);
        var expenses = await _getExpensesByCurrencyAndPeriodRepository.GetExpensesByCurrencyAndPeriodAsync(request);

        if (!expenses.Any())
        {
            throw new InvalidExpenseException("No expenses found in the specified period and category.");
        }

        var expensesResponse = expenses.Select(e => new ExpensesResponse
        {
            ExpenseId = e.Id,
            Amount = e.Amount,
            Currency = e.Currency,
            Category = e.Category,
            ExpenseDate = e.ExpenseDate
        }).ToList();

        return expensesResponse;
    }

    private void ValidateExpensesRequest(GetExpensesByCurrencyAndPeriodRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Expense period is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Expense currency is not valid.");
        }
    }
}

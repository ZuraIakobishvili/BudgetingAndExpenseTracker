using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;

public interface IGetExpensesByCategoryAndCurrencyInPeriodService
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriodAsync(GetExpensesByCategoryAndCurrencyInPeriodRequest request);
}
public class GetExpensesByCategoryAndCurrencyInPeriodService : IGetExpensesByCategoryAndCurrencyInPeriodService
{
    private readonly IGetExpensesByCategoryAndCurrencyInPeriodRepository _getExpensesByCategoryAndCurrencyInPeriodRepository;
    public GetExpensesByCategoryAndCurrencyInPeriodService(IGetExpensesByCategoryAndCurrencyInPeriodRepository getExpensesByCategoryAndCurrencyInPeriodRepository)
    {
        _getExpensesByCategoryAndCurrencyInPeriodRepository = getExpensesByCategoryAndCurrencyInPeriodRepository;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriodAsync(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        ValidateExpensesRequest(request);
        var expenses = await _getExpensesByCategoryAndCurrencyInPeriodRepository.GetExpensesByCategoryAndCurrencyInPeriodAsync(request);
        if (!expenses.Any())
        {
            throw new InvalidExpenseException("Expenses by category and currensy are not found in this period");
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

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Expense currency is not valid.");
        }

        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Expense category is not valid.");
        }
    }
}

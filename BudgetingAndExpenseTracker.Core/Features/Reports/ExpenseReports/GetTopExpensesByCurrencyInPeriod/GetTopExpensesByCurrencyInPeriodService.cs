using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;

public interface IGetTopExpensesByCurrencyInPeriodService
{
    Task<List<ExpensesResponse>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request);
}
public class GetTopExpensesByCurrencyInPeriodService : IGetTopExpensesByCurrencyInPeriodService
{
    private readonly IGetTopExpensesByCurrencyInPeriodRepositoy _getTopExpensesByCurrencyInPeriodRepositoy;
    public GetTopExpensesByCurrencyInPeriodService(IGetTopExpensesByCurrencyInPeriodRepositoy getTopExpensesByCurrencyInPeriodRepositoy)
    {
        _getTopExpensesByCurrencyInPeriodRepositoy = getTopExpensesByCurrencyInPeriodRepositoy;
    }

    public async Task<List<ExpensesResponse>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        ValidateTopExpensesRequest(request);
        var topExpenses = await _getTopExpensesByCurrencyInPeriodRepositoy.GetTopExpensesAsync(request);

        if (!topExpenses.Any())
        {
            throw new InvalidExpenseException("No expenses found.");
        }
        var expensesResponse = topExpenses.Select(e => new ExpensesResponse
        {
            ExpenseId = e.Id,
            Amount = e.Amount,
            Currency = e.Currency,
            Category = e.Category,
            ExpenseDate = e.ExpenseDate
        }).ToList();

        return expensesResponse;
    }

    private void ValidateTopExpensesRequest(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        if(request == null)
        {
             throw new ArgumentException(nameof(request));
        }

        if (request.TopExpensesCount <= 0)
        {
            throw new InvalidRequestException("Top number can not be zero or negative");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Expense currency is not valid.");
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Expense period is not valid.");
        }
    }
}

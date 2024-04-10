using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;

public interface IGetTopExpensesByCurrencyInPeriodService
{
    Task<List<Entities.Expense>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request);
}
public class GetTopExpensesByCurrencyInPeriodService : IGetTopExpensesByCurrencyInPeriodService
{
    private readonly IGetTopExpensesByCurrencyInPeriodRepositoy _getTopExpensesByCurrencyInPeriodRepositoy;
    public GetTopExpensesByCurrencyInPeriodService(IGetTopExpensesByCurrencyInPeriodRepositoy getTopExpensesByCurrencyInPeriodRepositoy)
    {
        _getTopExpensesByCurrencyInPeriodRepositoy = getTopExpensesByCurrencyInPeriodRepositoy;
    }

    public async Task<List<Entities.Expense>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        ValidateTopExpensesRequest(request);
        var topExpenses = await _getTopExpensesByCurrencyInPeriodRepositoy.GetTopExpensesAsync(request);

        if (!topExpenses.Any())
        {
            throw new InvalidExpenseException("No expenses found.");
        }

        return topExpenses;
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

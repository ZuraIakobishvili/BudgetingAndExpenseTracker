using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;

public interface IGetTopExpensesByCurrencyInPeriodService
{
    Task<List<Entities.Expense>> GetTopExpenses(GetTopExpensesByCurrencyInPeriodRequest request);
}
public class GetTopExpensesByCurrencyInPeriodService : IGetTopExpensesByCurrencyInPeriodService
{
    private readonly IGetTopExpensesByCurrencyInPeriodRepositoy _getTopExpensesByCurrencyInPeriodRepositoy;
    public GetTopExpensesByCurrencyInPeriodService(IGetTopExpensesByCurrencyInPeriodRepositoy getTopExpensesByCurrencyInPeriodRepositoy)
    {
        _getTopExpensesByCurrencyInPeriodRepositoy = getTopExpensesByCurrencyInPeriodRepositoy;
    }

    public async Task<List<Entities.Expense>> GetTopExpenses(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        IsTopExpensesCountIsValid(request.TopExpensesCount);
        var topExpenses = await _getTopExpensesByCurrencyInPeriodRepositoy.GetTopExpenses(request);

        if (!topExpenses.Any())
        {
            throw new InvalidExpenseException("No expenses found.");
        }

        return topExpenses;
    }

    private void IsTopExpensesCountIsValid(int count)
    {
        if (count <= 0)
        {
            throw new Exception("The count can not be null or negative.");
        }
    }
}

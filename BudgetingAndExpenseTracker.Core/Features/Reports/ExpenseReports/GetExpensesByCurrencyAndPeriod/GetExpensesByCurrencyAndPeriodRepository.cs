using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;

public interface IGetExpensesByCurrencyAndPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriod(GetExpensesByCurrencyAndPeriodRequest request);
}
public class GetExpensesByCurrencyAndPeriodRepository : IGetExpensesByCurrencyAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCurrencyAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriod(GetExpensesByCurrencyAndPeriodRequest request)
    {
        var expenses = await GetExpenses(request);

        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var expensesInPeriodByCurrency = expenses.Where(expense =>
            expense.ExpenseDate >= startDate &&
            expense.ExpenseDate <= endDate &&
            expense.Currency == request.Currency);

        if (!expensesInPeriodByCurrency.Any())
        {
            throw new NotFoundException("No expenses found in the specified period and currency.");
        }

        return expensesInPeriodByCurrency.ToList();
    }

    private async Task<List<Entities.Expense>> GetExpenses(GetExpensesByCurrencyAndPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }
}


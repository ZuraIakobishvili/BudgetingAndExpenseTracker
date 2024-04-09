using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;

public interface IGetTopExpensesByCurrencyInPeriodRepositoy
{
    Task<List<Entities.Expense>> GetTopExpenses(GetTopExpensesByCurrencyInPeriodRequest request);
}
public class GetTopExpensesByCurrencyInPeriodRepositoy : IGetTopExpensesByCurrencyInPeriodRepositoy
{
    private readonly IDbConnection _dbConnection;
    public GetTopExpensesByCurrencyInPeriodRepositoy(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetTopExpenses(GetTopExpensesByCurrencyInPeriodRequest request)
    {
      //  IsTopExpensesCountIsValid(request.TopExpensesCount);
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;
        var expenses = await GetExpenses(request);
        var expensesByCurrency = expenses.Where(expense => expense.Currency == request.Currency);

        var topExpenses = expensesByCurrency
            .OrderByDescending(expense => expense.Amount)
            .Take(request.TopExpensesCount);

        return topExpenses.ToList();
    }

    private async Task<List<Entities.Expense>> GetExpenses(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }
}

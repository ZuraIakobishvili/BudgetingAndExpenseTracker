using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;

public interface IGetTopExpensesByCurrencyInPeriodRepositoy
{
    Task<List<Entities.Expense>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request);
}
public class GetTopExpensesByCurrencyInPeriodRepositoy : IGetTopExpensesByCurrencyInPeriodRepositoy
{
    private readonly IDbConnection _dbConnection;
    public GetTopExpensesByCurrencyInPeriodRepositoy(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetTopExpensesAsync(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var query = @"
        WITH UserExpenses AS (
            SELECT Id, UserId, Amount, Currency, Category, ExpenseDate
            FROM Expenses
            WHERE UserId = @UserId
                AND ExpenseDate >= @StartDate
                AND ExpenseDate <= @EndDate
        )
        SELECT TOP (@TopExpensesCount) Id, UserId, Amount, Currency, Category, ExpenseDate
        FROM UserExpenses
        WHERE Currency = @Currency
        ORDER BY Amount DESC;";

        var parameters = new
        {
            request.UserId,
            request.Currency,
            request.TopExpensesCount,
            StartDate = startDate,
            EndDate = endDate
        };

        return (await _dbConnection.QueryAsync<Entities.Expense>(query, parameters)).ToList();
    }
}
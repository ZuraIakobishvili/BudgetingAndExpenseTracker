using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;

public interface IGetExpensesByCategoryAndCurrencyInPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriod(GetExpensesByCategoryAndCurrencyInPeriodRequest request);
}
public class GetExpensesByCategoryAndCurrencyInPeriodRepository : IGetExpensesByCategoryAndCurrencyInPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCategoryAndCurrencyInPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriod(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        var expenses = await GetExpenses(request);
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;


        var expensesInPeriodByCategoryAndCurrency = expenses
            .Where(expense => expense.ExpenseDate >= startDate && expense.ExpenseDate <= endDate)
            .Where(expense => expense.Category == request.Category && expense.Currency == request.Currency);

        return expensesInPeriodByCategoryAndCurrency.ToList();
    }

    private async Task<List<Entities.Expense>> GetExpenses(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }
}

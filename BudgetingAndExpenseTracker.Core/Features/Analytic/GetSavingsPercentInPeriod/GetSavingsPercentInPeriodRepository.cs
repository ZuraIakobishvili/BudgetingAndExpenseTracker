using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetSavingsPercentInPeriod;

public interface IGetSavingsPercentInPeriodRepository
{
    Task<decimal> GetSavingsPercentInPeriodAsync(GetSavingsPercentInPeriodRequest request);
}
public class GetSavingsPercentInPeriodRepository : IGetSavingsPercentInPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetSavingsPercentInPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<decimal> GetSavingsPercentInPeriodAsync(GetSavingsPercentInPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;
        var savings = await GetSavingsAsync(request);
        var incomes = await GetIncomesAsync(request);

        var totalIncome = incomes
           .Where(income => income.IncomeDate >= startDate && income.IncomeDate <= endDate)
           .Where(income => income.Currency == request.Currency)
           .Sum(income => income.Amount);

        if (totalIncome > 0)
        {
            var savingPercentInPeriod = Math.Round((savings / totalIncome) * 100);
            return savingPercentInPeriod;
        }
        else
        {
            return 0;
        }
    }

    private async Task<List<Entities.Income>> GetIncomesAsync(GetSavingsPercentInPeriodRequest request)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new {request.UserId })).ToList();
    }

    private async Task<List<Entities.Expense>> GetExpensesAsync(GetSavingsPercentInPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }

    private async Task<decimal> GetSavingsAsync(GetSavingsPercentInPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var totalIncomes = await GetIncomesAsync(request);
        var totalExpenses = await GetExpensesAsync(request);

        var totalIncomesInPeriod = totalIncomes
            .Where(income => income.IncomeDate >= startDate && income.IncomeDate <= endDate)
            .Where(income => income.Currency == request.Currency)
            .Sum(income => income.Amount);

        var totalExpensesInPeriod = totalExpenses
            .Where(expense => expense.ExpenseDate >= startDate && expense.ExpenseDate <= endDate)
            .Where(expense => expense.Currency == request.Currency)
            .Sum(expense => expense.Amount);

        var totalSaving = totalIncomesInPeriod - totalExpensesInPeriod;
        return totalSaving;
    }

}

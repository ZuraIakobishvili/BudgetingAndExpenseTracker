﻿using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;

public interface IGetTotalSavingsInPeriodRepository
{
    Task<decimal> GetTotalSavingsAsync(GetTotalSavingsInPeriodRequest request);
}
public class GetTotalSavingsInPeriodRepository : IGetTotalSavingsInPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetTotalSavingsInPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<decimal> GetTotalSavingsAsync(GetTotalSavingsInPeriodRequest request)
    {
        var incomes = await GetIncomesAsync(request);
        var expenses = await GetExpensesAsync(request);

        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var totalIncomesInPeriod = incomes
            .Where(income => income.IncomeDate >= startDate && income.IncomeDate <= endDate)
            .Where(income => income.Currency == request.Currency)
            .Sum(income => income.Amount);

        var totalExpensesInPeriod = expenses
           .Where(expense => expense.ExpenseDate >= startDate && expense.ExpenseDate <= endDate)
           .Where(expense => expense.Currency == request.Currency)
           .Sum(expense => expense.Amount);

        var totalSaving = totalIncomesInPeriod - totalExpensesInPeriod;
        return totalSaving;
    }


    private async Task<List<Entities.Income>> GetIncomesAsync(GetTotalSavingsInPeriodRequest request)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new { request.UserId })).ToList();
    }

    private async Task<List<Entities.Expense>> GetExpensesAsync(GetTotalSavingsInPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }

}
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;

public interface IGetExpensesByCategoryAndCurrencyInPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriodAsync(GetExpensesByCategoryAndCurrencyInPeriodRequest request);
}
public class GetExpensesByCategoryAndCurrencyInPeriodRepository : IGetExpensesByCategoryAndCurrencyInPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCategoryAndCurrencyInPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndCurrencyInPeriodAsync(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var query = @"
                SELECT * FROM Expenses
                WHERE UserId = @UserId
                    AND Category = @Category 
                    AND Currency = @Currency 
                    AND ExpenseDate  >= @StartDate  
                    AND ExpenseDate  <= @EndDate";

        var parameters = new
        {
            request.UserId,
            request.Category,
            request.Currency,
            StartDate = startDate,
            EndDate = endDate
        };

        return (await _dbConnection.QueryAsync<Entities.Expense>(query, parameters)).ToList();
    }
}


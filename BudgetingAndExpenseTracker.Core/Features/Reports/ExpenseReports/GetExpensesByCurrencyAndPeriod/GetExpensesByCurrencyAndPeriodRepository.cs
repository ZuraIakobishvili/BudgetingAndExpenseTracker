using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;

public interface IGetExpensesByCurrencyAndPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriodAsync(GetExpensesByCurrencyAndPeriodRequest request);
}

public class GetExpensesByCurrencyAndPeriodRepository : IGetExpensesByCurrencyAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCurrencyAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCurrencyAndPeriodAsync(GetExpensesByCurrencyAndPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var query = @"
                SELECT * FROM Expenses
                WHERE UserId = @UserId
                    AND Currency = @Currency 
                    AND ExpenseDate  >= @StartDate  
                    AND ExpenseDate  <= @EndDate";

        var parameters = new
        {
            request.UserId,
            request.Currency,
            StartDate = startDate,
            EndDate = endDate
        };

        return (await _dbConnection.QueryAsync<Entities.Expense>(query, parameters)).ToList();
    }
}


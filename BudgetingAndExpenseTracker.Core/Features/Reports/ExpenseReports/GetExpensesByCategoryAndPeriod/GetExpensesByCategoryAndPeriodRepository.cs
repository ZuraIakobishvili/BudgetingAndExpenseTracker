using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
public interface IGetExpensesByCategoryAndPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriodAsync(GetExpensesByCategoryAndPeriodRequest request);
}

public class GetExpensesByCategoryAndPeriodRepository : IGetExpensesByCategoryAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCategoryAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriodAsync(GetExpensesByCategoryAndPeriodRequest request)
    {
        using (_dbConnection)
        {
            var startDate = UserHelper.GetStartDay(request.Period);
            var endDate = DateTime.Now;

            var query = @"
                SELECT * FROM Expenses
                WHERE UserId = @UserId
                    AND Category = @Category 
                    AND ExpenseDate  >= @ExpenseDate  
                    AND ExpenseDate  <= @EndDate";

            var parameters = new
            {
                request.UserId,
                request.Category,
                StarDate = startDate,
                EndDate = endDate
            };

            return (await _dbConnection.QueryAsync<Entities.Expense>(query, parameters)).ToList();
        }
    }
    
}

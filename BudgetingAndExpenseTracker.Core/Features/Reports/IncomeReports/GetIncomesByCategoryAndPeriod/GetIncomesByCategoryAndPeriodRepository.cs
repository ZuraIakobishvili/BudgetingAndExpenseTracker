using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodRepository
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request);
}

public class GetIncomesByCategoryAndPeriodRepository : IGetIncomesByCategoryAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCategoryAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request)
    {
        using (_dbConnection)
        {
            var startDate = UserHelper.GetStartDay(request.Period);
            var endDate = DateTime.Now;

            var query = @"
                SELECT * FROM Incomes
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

            return (await _dbConnection.QueryAsync<Entities.Income>(query, parameters)).ToList();
        }
    }
}

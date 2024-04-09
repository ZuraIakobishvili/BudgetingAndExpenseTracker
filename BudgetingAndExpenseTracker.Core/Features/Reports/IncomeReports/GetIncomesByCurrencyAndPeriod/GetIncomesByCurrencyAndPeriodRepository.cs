using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;

public interface IGetIncomesByCurrencyAndPeriodRepository
{
    Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriod(GetIncomesByCurrencyAndPeriodRequest request);
}
public class GetIncomesByCurrencyAndPeriodRepository : IGetIncomesByCurrencyAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCurrencyAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriod(GetIncomesByCurrencyAndPeriodRequest request)
    {
        using (_dbConnection)
        {
            var startDate = UserHelper.GetStartDay(request.Period);
            var endDate = DateTime.Now;

            var query = @"
                SELECT * FROM Expenses
                WHERE UserId = @UserId
                    AND Currency = @Currency 
                    AND ExpenseDate  >= @ExpenseDate  
                    AND ExpenseDate  <= @EndDate";

            var parameters = new
            {
                request.UserId,
                request.Currency,
                StarDate = startDate,
                EndDate = endDate
            };

            return (await _dbConnection.QueryAsync<Entities.Income>(query, parameters)).ToList();
        }
    }
}

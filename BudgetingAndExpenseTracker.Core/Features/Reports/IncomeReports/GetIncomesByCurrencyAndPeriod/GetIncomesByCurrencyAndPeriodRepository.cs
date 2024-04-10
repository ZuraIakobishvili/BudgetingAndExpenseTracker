using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;

public interface IGetIncomesByCurrencyAndPeriodRepository
{
    Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriodAsync(GetIncomesByCurrencyAndPeriodRequest request);
}
public class GetIncomesByCurrencyAndPeriodRepository : IGetIncomesByCurrencyAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCurrencyAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Income>> GetIncomesByCurrencyAndPeriodAsync(GetIncomesByCurrencyAndPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var query = @"
                SELECT * FROM Incomes
                WHERE UserId = @UserId
                    AND Currency = @Currency 
                    AND IncomeDate  >= @StartDate  
                    AND IncomeDate  <= @EndDate";

        var parameters = new
        {
            request.UserId,
            request.Currency,
            StartDate = startDate,
            EndDate = endDate
        };

        return (await _dbConnection.QueryAsync<Entities.Income>(query, parameters)).ToList();
    }
}

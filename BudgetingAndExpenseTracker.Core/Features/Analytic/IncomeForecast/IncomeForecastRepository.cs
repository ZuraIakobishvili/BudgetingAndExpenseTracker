using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;

public interface IIncomeForecastRepository
{
    Task<decimal> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request);
}

public class IncomeForecastRepository : IIncomeForecastRepository
{
    private readonly IDbConnection _dbConnection;

    public IncomeForecastRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<decimal> GetNextMonthIncomeForecastAsync(IncomeForecastRequest request)
    {
        var coefficient = ForecastCoefficient.IncomeForecastCoefficient;
        var monthsCount = await GetMonthCountAsync(request.UserId);

        var query = @"
            SELECT SUM(Amount) AS TotalAmount
            FROM Incomes
            WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND DATEPART(YEAR, IncomeDate) = DATEPART(YEAR, GETDATE())";

        var parameters = new
        {
            request.UserId,
            request.Category,
            request.Currency
        };

        var totalIncome = await _dbConnection.QueryFirstOrDefaultAsync<decimal?>(query, parameters);

        if (totalIncome == null)
        {
            return 0;
        }

        var averageIncomePerMonthWithCoefficient = Math.Round(totalIncome.Value / monthsCount * coefficient, 2);

        return averageIncomePerMonthWithCoefficient;
    }

    private async Task<int> GetMonthCountAsync(string userId)
    {
        var query = "SELECT MIN(IncomeDate) AS FirstIncomeDate FROM Incomes WHERE UserId = @UserId";
        var firstIncomeDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime?>(query, new { userId });

        if (firstIncomeDate == null)
        {
            return 1;
        }

        var startDate = firstIncomeDate.Value;
        var endDate = DateTime.UtcNow;
        var monthCount = MonthCount(startDate, endDate);

        return monthCount;
    }

    private int MonthCount(DateTime startDate, DateTime endDate)
    {
        int monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

        if (endDate.Day < startDate.Day)
        {
            monthCount--;
        }

        if (monthCount == 0)
        {
            return 1;
        }

        return monthCount;
    }
}
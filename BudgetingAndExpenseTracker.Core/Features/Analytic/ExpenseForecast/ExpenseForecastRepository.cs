using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;

public interface IExpenseForecastRepository
{
    Task<decimal> GetNextMonthExpenseForecastAsync(ExpenseForecastRequest request);
}

public class ExpenseForecastRepository : IExpenseForecastRepository
{
    private readonly IDbConnection _dbConnection;

    public ExpenseForecastRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<decimal> GetNextMonthExpenseForecastAsync(ExpenseForecastRequest request)
    {
        var coefficient = ForecastCoefficient.ExpenseForecastCoefficient;
        var monthsCount = await GetMonthCountAsync(request.UserId);

        var query = @"
            SELECT SUM(Amount) AS TotalAmount
            FROM Expenses
            WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND DATEPART(YEAR, ExpenseDate) = DATEPART(YEAR, GETDATE())";

        var parameters = new
        {
            request.UserId,
            request.Category,
            request.Currency
        };

        var totalExpense = await _dbConnection.QueryFirstOrDefaultAsync<decimal?>(query, parameters);

        if (totalExpense == null)
        {
            return 0;
        }

        var expenseForecast = Math.Round(totalExpense.Value / monthsCount * coefficient, 2);

        return expenseForecast;

    }

    private async Task<int> GetMonthCountAsync(string userId)
    {
        var firstExpenseDateQuery = "SELECT MIN(ExpenseDate) AS FirstExpenseDate FROM Expenses WHERE UserId = @UserId";
        var firstExpenseDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime?>(firstExpenseDateQuery, new { userId });

        if (firstExpenseDate == null)
        {
            return 1;
        }

        var startDate = firstExpenseDate.Value;
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
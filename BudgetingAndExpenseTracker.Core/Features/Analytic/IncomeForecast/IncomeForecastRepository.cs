using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;

public interface IIncomeForecastRepository
{
    Task<Dictionary<string, decimal>> GetNextMonthIncomeForecast(IncomeForecastRequest request);
}

public class IncomeForecastRepository : IIncomeForecastRepository
{
    private readonly IDbConnection _dbConnection;

    public IncomeForecastRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Dictionary<string, decimal>> GetNextMonthIncomeForecast(IncomeForecastRequest request)
    {
        var coefficient = ForecastCoefficient.IncomeForecastCoefficient;
        var monthsCount = await GetMonthCount(request);

        var query = @"
            SELECT Category, Currency, SUM(Amount) AS TotalAmount
            FROM Incomes
            WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND DATEPART(YEAR, IncomeDate) = DATEPART(YEAR, GETDATE())
            GROUP BY Category, Currency";

        var parameters = new
        {
            request.UserId,
            request.Category,
            request.Currency
        };

        var incomesByCategoryAndCurrency = (await _dbConnection.QueryAsync<dynamic>(query, parameters)).ToList();

        if (!incomesByCategoryAndCurrency.Any())
        {
            return new Dictionary<string, decimal>();
        }

        var averageIncomesByCategoryAndCurrency = new Dictionary<string, decimal>();

        foreach (var expense in incomesByCategoryAndCurrency)
        {
            var category = (ExpenseCategory)expense.Category;
            var currency = (Currency)expense.Currency;
            var totalExpenseForCategory = (decimal)expense.TotalAmount;
            var averageIncomePerMonthWithCoefficient = Math.Round((totalExpenseForCategory / monthsCount) * coefficient, 2);

            var key = $"{category}_{currency}";
            if (!averageIncomesByCategoryAndCurrency.TryGetValue(key, out _))
            {
                averageIncomesByCategoryAndCurrency[key] = 0;
            }

            averageIncomesByCategoryAndCurrency[key] += averageIncomePerMonthWithCoefficient;
        }

        return averageIncomesByCategoryAndCurrency;
    }

    private async Task<int> GetMonthCount(IncomeForecastRequest request)
    {
        var query = "SELECT MIN(IncomeDate) AS FirstIncomeDate FROM Incomes WHERE UserId = @UserId";
        var firstIncomeDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime?>(query, new { request.UserId });

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

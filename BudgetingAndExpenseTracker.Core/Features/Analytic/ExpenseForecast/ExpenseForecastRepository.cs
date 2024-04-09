using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;

public interface IExpenseForecastRepository
{
    Task<Dictionary<string, decimal>> GetNextMonthExpenseForecast(ExpenseForecastRequest request);
}

public class ExpenseForecastRepository : IExpenseForecastRepository
{
    private readonly IDbConnection _dbConnection;

    public ExpenseForecastRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Dictionary<string, decimal>> GetNextMonthExpenseForecast(ExpenseForecastRequest request)
    {
        var coefficient = ForecastCoefficient.ExpenseForecastCoefficient;
        var monthsCount = await GetMonthCount(request);

        var query = @"
            SELECT Category, Currency, SUM(Amount) AS TotalAmount
            FROM Expenses
            WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND DATEPART(YEAR, ExpenseDate) = DATEPART(YEAR, GETDATE())
            GROUP BY Category, Currency";

        var parameters = new
        {
            request.UserId,
            request.Category,
            request.Currency
        };

        var expensesByCategoryAndCurrency = (await _dbConnection.QueryAsync<dynamic>(query, parameters)).ToList();

        if (!expensesByCategoryAndCurrency.Any())
        {
            return new Dictionary<string, decimal>();
        }

        var averageExpensesByCategoryAndCurrency = new Dictionary<string, decimal>();

        foreach (var expense in expensesByCategoryAndCurrency)
        {
            var category = (ExpenseCategory)expense.Category;
            var currency = (Currency)expense.Currency;
            var totalExpenseForCategory = (decimal)expense.TotalAmount;
            var averageExpensePerMonthWithCoefficient = Math.Round((totalExpenseForCategory / monthsCount) * coefficient, 2);

            var key = $"{category}_{currency}";
            if (!averageExpensesByCategoryAndCurrency.TryGetValue(key, out _))
            {
                averageExpensesByCategoryAndCurrency[key] = 0;
            }

            averageExpensesByCategoryAndCurrency[key] += averageExpensePerMonthWithCoefficient;
        }

        return averageExpensesByCategoryAndCurrency;
    }

    private async Task<int> GetMonthCount(ExpenseForecastRequest request)
    {
        var query = "SELECT MIN(ExpenseDate) AS FirstExpenseDate FROM Expenses WHERE UserId = @UserId";
        var firstExpenseDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime?>(query, new { request.UserId });

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
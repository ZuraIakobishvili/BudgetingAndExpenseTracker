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
        var incomes = await GetIncomes(request);
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var incomesByCurrency = incomes.Where(income =>
           income.IncomeDate >= startDate &&
           income.IncomeDate <= endDate &&
           income.Currency == request.Currency);

        return incomesByCurrency.ToList();
    }

    private async Task<List<Entities.Income>> GetIncomes(GetIncomesByCurrencyAndPeriodRequest request)
    {
        var query = "SELECT * FROM Incomes Where UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new { request.UserId})).ToList();

    }
}

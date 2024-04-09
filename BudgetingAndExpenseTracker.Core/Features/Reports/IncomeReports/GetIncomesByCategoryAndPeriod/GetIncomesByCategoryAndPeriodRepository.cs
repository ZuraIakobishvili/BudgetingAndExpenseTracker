using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodRepository
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndPeriod(GetIncomesByCategoryAndPeriodRequest request);
}

public class GetIncomesByCategoryAndPeriodRepository : IGetIncomesByCategoryAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCategoryAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Income>> GetIncomesByCategoryAndPeriod(GetIncomesByCategoryAndPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;
        var incomes = await GetIncomes(request);

        var incomesByCategoryAndPeriod = incomes.Where(income =>
           income.IncomeDate >= startDate &&
           income.IncomeDate <= endDate &&
           income.Category == request.Category);

        return incomesByCategoryAndPeriod.ToList();
    }

    private async Task<List<Entities.Income>> GetIncomes(GetIncomesByCategoryAndPeriodRequest request)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new { request.UserId })).ToList();
    }
}

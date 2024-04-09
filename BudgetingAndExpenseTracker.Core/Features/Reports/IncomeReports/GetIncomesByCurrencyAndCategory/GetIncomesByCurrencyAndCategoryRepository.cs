using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
public interface IGetIncomesByCurrencyAndCategoryRepository
{
    Task<List<Entities.Income>> GetIncomesByCurrencyAndCategory(GetIncomesByCurrencyAndCategoryRequest request);
}

public class GetIncomesByCurrencyAndCategoryRepository : IGetIncomesByCurrencyAndCategoryRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCurrencyAndCategoryRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.Income>> GetIncomesByCurrencyAndCategory(GetIncomesByCurrencyAndCategoryRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var incomes = await GetIncomes(request);
        var incomesInPeriodByCurrencyAndCategory = incomes
            .Where(income => income.IncomeDate >= startDate && income.IncomeDate <= endDate)
            .Where(income => income.Currency == request.Currency && income.Category == request.Category);

        return incomesInPeriodByCurrencyAndCategory.ToList();
    }

    private async Task<List<Entities.Income>> GetIncomes(GetIncomesByCurrencyAndCategoryRequest request)
    {
        var query = "SELECT * FROM Income WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new {request.UserId })).ToList();
    }
}

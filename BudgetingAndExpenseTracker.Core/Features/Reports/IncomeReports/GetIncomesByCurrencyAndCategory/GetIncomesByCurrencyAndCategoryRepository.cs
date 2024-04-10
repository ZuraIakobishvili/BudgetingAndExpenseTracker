using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
public interface IGetIncomesByCurrencyAndCategoryRepository
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndCurrencyInPeriodAsync(GetIncomesByCurrencyAndCategoryRequest request);
}

public class GetIncomesByCurrencyAndCategoryRepository : IGetIncomesByCurrencyAndCategoryRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCurrencyAndCategoryRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.Income>> GetIncomesByCategoryAndCurrencyInPeriodAsync(GetIncomesByCurrencyAndCategoryRequest request)
    {
        using (_dbConnection)
        {
            var startDate = UserHelper.GetStartDay(request.Period);
            var endDate = DateTime.Now;

            var query = @"
                SELECT * FROM Expenses
                WHERE UserId = @UserId
                    AND Category = @Category 
                    AND Currency = @Currency 
                    AND ExpenseDate  >= @ExpenseDate  
                    AND ExpenseDate  <= @EndDate";

            var parameters = new
            {
                request.UserId,
                request.Category,
                request.Currency,
                StarDate = startDate,
                EndDate = endDate
            };

            return (await _dbConnection.QueryAsync<Entities.Income>(query, parameters)).ToList();
        }
    }
}

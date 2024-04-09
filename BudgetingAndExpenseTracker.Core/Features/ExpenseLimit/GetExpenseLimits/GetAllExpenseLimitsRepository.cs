
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.GetExpenseLimits;
public interface IGetAllExpenseLimitsRepository
{
    Task<List<Entities.ExpenseLimit>> GetExpenseLimits(GetAllExpenseLimitsRequest request);
}
public class GetAllExpenseLimitsRepository : IGetAllExpenseLimitsRepository
{
    private readonly IDbConnection _dbConnection;
    public GetAllExpenseLimitsRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.ExpenseLimit>> GetExpenseLimits(GetAllExpenseLimitsRequest request)
    {
        var query = "SELECT * FROM Limits WHERE UserId = @UserId";
        return (List<Entities.ExpenseLimit>)await _dbConnection.QueryAsync<Entities.ExpenseLimit>(query, new { request.UserId });
    }
}

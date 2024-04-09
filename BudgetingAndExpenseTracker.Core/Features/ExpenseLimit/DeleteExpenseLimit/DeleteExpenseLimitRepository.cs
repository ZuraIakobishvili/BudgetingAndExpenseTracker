using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.DeleteExpenseLimit;

public interface IDeleteExpenseLimitRepository
{
    Task<bool> Delete(DeleteExpenseLimitRequest request);
}

public class DeleteExpenseLimitRepository : IDeleteExpenseLimitRepository
{
    private readonly IDbConnection _dbConnection;
    public DeleteExpenseLimitRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<bool> Delete(DeleteExpenseLimitRequest request)
    {
        var query = "DELETE FROM Limits WHERE Id = @Id AND UserId = @UserId";
        var success = await _dbConnection.ExecuteAsync(query, new { Id = request.LimitId, UserId = request.UserId });
        return success > 0;
    }
}

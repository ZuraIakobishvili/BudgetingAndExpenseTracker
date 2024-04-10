using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;
public interface IDeleteIncomeRepository
{
    Task<bool> DeleteIncomeAsync(DeleteIncomeRequest request);
}

public class DeleteIncomeRepository : IDeleteIncomeRepository
{
    private readonly IDbConnection _dbConnection;
    public DeleteIncomeRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> DeleteIncomeAsync(DeleteIncomeRequest request)
    {
        var query = "DELETE FROM Incomes WHERE Id = @Id AND UserId = @UserId";
        var success = await _dbConnection.ExecuteAsync(query, new { Id = request.IncomeId, UserId = request.UserId});
        return success > 0;
    }
}

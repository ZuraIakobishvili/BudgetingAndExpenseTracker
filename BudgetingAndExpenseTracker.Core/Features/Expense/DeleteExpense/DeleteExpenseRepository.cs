using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;

public interface IDeleteExpenseRepository
{
    Task<bool> DeleteExpenseAsync(DeleteExpenseRequest request);
}
public class DeleteExpenseRepository : IDeleteExpenseRepository
{
    private readonly IDbConnection _dbConnection;
    public DeleteExpenseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<bool> DeleteExpenseAsync(DeleteExpenseRequest request)
    {
        var query = "DELETE FROM Expenses WHERE Id = @Id AND UserId = @UserId";
        var success = await _dbConnection.ExecuteAsync(query, new { Id = request.ExpenseId, UserId = request.UserId });
        return success > 0;
    }
}

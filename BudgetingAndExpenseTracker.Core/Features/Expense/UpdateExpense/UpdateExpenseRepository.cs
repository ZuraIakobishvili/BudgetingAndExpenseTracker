using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;

public interface IUpdateExpenseRepository
{
    Task<bool> Update(UpdateExpenseRequest request);
}
public class UpdateExpenseRepository : IUpdateExpenseRepository
{
    private readonly IDbConnection _dbConnection;
    public UpdateExpenseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<bool> Update(UpdateExpenseRequest request)
    {

        var query = "UPDATE Expenses SET Amount = @Amount, Currency = @Currency, Category = @Category WHERE Id = @Id AND UserId = @UserId";
        var rowsAffected = await _dbConnection.ExecuteAsync(query, new
        {
            request.UserId,
            request.Amount,
            request.Currency,
            request.Category,
        });

        return rowsAffected > 0;
    }
}

using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;

public interface IUpdateExpenseLimitRepository
{
    Task<bool> UpdateLimitAsync(UpdateExpenseLimitRequest request);
}

public class UpdateExpenseLimitRepository : IUpdateExpenseLimitRepository
{
    private readonly IDbConnection _dbConnection;
    public UpdateExpenseLimitRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> UpdateLimitAsync(UpdateExpenseLimitRequest request)
    {
        var query = "UPDATE Limits SET Amount = @Amount, Currency = @Currency, Category = @Category, LimitPeriod = @Period WHERE Id = @Id AND UserId = @UserId";
        var rowsAffected = await _dbConnection.ExecuteAsync(query, new
        {
            request.Id,
            request.UserId,
            request.Amount,
            request.Currency,
            request.Category,
            request.Period
        });

        return rowsAffected > 0;
    }
}

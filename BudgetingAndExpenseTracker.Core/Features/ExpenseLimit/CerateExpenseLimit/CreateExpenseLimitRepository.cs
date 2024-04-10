using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit; 
public interface ICreateExpenseLimitRepository
{
    Task<bool> CreateExpenseLimitAsync(CreateExpenseLimitRequest request);
}

public class CreateExpenseLimitRepository : ICreateExpenseLimitRepository
{
    private readonly IDbConnection _dbConnection;
    public CreateExpenseLimitRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<bool> CreateExpenseLimitAsync(CreateExpenseLimitRequest request)
    {

        var query = "INSERT INTO Limits (Id, UserId, Amount, Currency, Category, LimitPeriod) " +
                   "VALUES (@Id, @UserId, @Amount, @Currency, @Category, @LimitPeriod)";

        Entities.ExpenseLimit expenseLimit = new()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Amount = request.Amount,
            Category = request.Category,
            Currency = request.Currency,
            LimitPeriod = request.LimitPeriod
        };

        return await _dbConnection.ExecuteAsync(query, expenseLimit) > 0;
    }
}

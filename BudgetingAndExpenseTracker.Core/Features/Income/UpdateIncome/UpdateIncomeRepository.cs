using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;

public interface IUpdateIncomeRepository
{
    Task<bool> UpdateIncomeAsync(UpdateIncomeRequest request);
}


public class UpdateIncomeRepository : IUpdateIncomeRepository
{
    private readonly IDbConnection _dbConnection;
    public UpdateIncomeRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<bool> UpdateIncomeAsync(UpdateIncomeRequest request)
    {
        var query = "UPDATE Incomes SET Amount = @Amount, Currency = @Currency, Category = @Category WHERE Id = @Id AND @UserId = UserId";
        var rowsAffected = await _dbConnection.ExecuteAsync(query, new
        {
            request.Id,
            request.UserId,
            request.Amount,
            request.Currency,
            request.Category
        });

        return rowsAffected > 0;
    }
}

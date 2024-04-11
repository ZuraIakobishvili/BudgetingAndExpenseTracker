using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;

public interface ICreateIncomeRepository
{
    Task<bool> CreateIncomeAsync(CreateIncomeRequest request);
}
public class CreateIncomeRepository : ICreateIncomeRepository
{
    private readonly IDbConnection _dbConnection;
    public CreateIncomeRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> CreateIncomeAsync(CreateIncomeRequest request)
    {
        var query = "INSERT INTO Incomes (Id, UserId, Amount, Currency, Category, IncomeDate) VALUES (@Id, @UserId, @Amount, @Currency, @Category, @IncomeDate)";

        Entities.Income income = new()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            IncomeDate = DateTime.UtcNow,
            Category = request.Category,
            Currency = request.Currency,
            Amount = request.Amount
        };
        return await _dbConnection.ExecuteAsync(query, income) > 0;
    }
}
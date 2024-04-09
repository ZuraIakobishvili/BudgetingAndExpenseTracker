using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;

public interface ICreateExpenseRepository
{
    Task<bool> Create(CreateExpenseRequest request);
}
public class CreateExpenseRepository : ICreateExpenseRepository
{
    private readonly IDbConnection _dbConnection;
    public CreateExpenseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> Create(CreateExpenseRequest request)
    {
        var query = "INSERT INTO Expenses (Id, UserId, Amount, Currency, Category, ExpenseDate) " +
                   "VALUES (@Id, @UserId, @Amount, @Currency, @Category, @ExpenseDate)";

        Entities.Expense expense = new()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ExpenseDate = DateTime.UtcNow,
            Category = request.Category,
            Currency = request.Currency,
            Amount = request.Amount
        };

        return await _dbConnection.ExecuteAsync(query, expense) > 0;
    }
}

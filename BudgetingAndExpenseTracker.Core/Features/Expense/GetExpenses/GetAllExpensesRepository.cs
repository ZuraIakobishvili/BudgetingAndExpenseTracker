using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.GetExpenses;

public interface IGetAllExpensesRepository
{
    Task<List<Entities.Expense>> GetExpenses(GetAllExpensesRequest request); 
}
public class GetAllExpensesRepository : IGetAllExpensesRepository
{
    private readonly IDbConnection _dbConnection;
    public GetAllExpensesRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.Expense>> GetExpenses(GetAllExpensesRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (List<Entities.Expense>)await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId });
    }
}

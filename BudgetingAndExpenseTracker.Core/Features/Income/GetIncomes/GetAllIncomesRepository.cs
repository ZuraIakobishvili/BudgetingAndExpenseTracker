using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Income.GetIncomes;

public interface IGetAllIncomesRepository
{
    Task<List<Entities.Income>> GetIncomes(GetAllIncomesRequest request);
}
public class GetAllIncomesRepository : IGetAllIncomesRepository
{
    private readonly IDbConnection _dbConnection;
    public GetAllIncomesRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.Income>> GetIncomes(GetAllIncomesRequest request)
    {
        var query = "SELECT * FROM Limits WHERE UserId = @UserId";
        return (List<Entities.Income>)await _dbConnection.QueryAsync<Entities.Income>(query, new { request.UserId });
    }
}

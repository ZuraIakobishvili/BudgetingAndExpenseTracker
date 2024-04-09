using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
public interface IGetExpensesByCategoryAndPeriodRepository
{
    Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriod(GetExpensesByCategoryAndPeriodRequest request);
}

public class GetExpensesByCategoryAndPeriodRepository : IGetExpensesByCategoryAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetExpensesByCategoryAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<List<Entities.Expense>> GetExpensesByCategoryAndPeriod(GetExpensesByCategoryAndPeriodRequest request)
    {
        var expenses = await GetExpenses(request);

        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var expensesInPeriodByCurrency = expenses.Where(expense =>
            expense.ExpenseDate >= startDate &&
            expense.ExpenseDate <= endDate &&
            expense.Category == request.Category);

        return expensesInPeriodByCurrency.ToList();
    }
      
    private async Task<List<Entities.Expense>> GetExpenses(GetExpensesByCategoryAndPeriodRequest request)
    {
        var query = "SELECT * FROM Expenses WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Expense>(query, new { request.UserId })).ToList();
    }
}

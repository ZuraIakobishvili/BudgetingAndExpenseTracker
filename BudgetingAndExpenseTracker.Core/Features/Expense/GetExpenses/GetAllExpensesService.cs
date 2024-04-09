using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.GetExpenses;
public interface IGetAllExpensesService
{
    Task<List<Entities.Expense>> GetExpenses(GetAllExpensesRequest request);
}
public class GetAllExpensesService : IGetAllExpensesService
{
    private readonly IGetAllExpensesRepository _allExpensesRepository;
    public GetAllExpensesService(IGetAllExpensesRepository getAllExpensesRepository)
    {
         _allExpensesRepository = getAllExpensesRepository;
    }
    public async Task<List<Entities.Expense>> GetExpenses(GetAllExpensesRequest request)
    {
        var expenses = await _allExpensesRepository.GetExpenses(request);
        if(expenses.Count == 0)
        {
            throw new InvalidExpenseException("Expenses List is empty");
        }
        return expenses;
    }
}

using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;

public interface IDeleteExpenseService
{
    Task<DeleteExpenseResponse> DeleteExpense(DeleteExpenseRequest request);
}
public class DeleteExpenseService : IDeleteExpenseService
{
    private readonly IDeleteExpenseRepository _deleteExpenseRepository;
    public DeleteExpenseService(IDeleteExpenseRepository deleteExpenseRepositor)
    {
        _deleteExpenseRepository = deleteExpenseRepositor;
    }
    public async Task<DeleteExpenseResponse> DeleteExpense(DeleteExpenseRequest request)
    {
        var deletedExpense = await _deleteExpenseRepository.DeleteExpense(request);
        if (!deletedExpense)
        {
            throw new InvalidExpenseException("Expenses can not be deleted.");
        }

        return new DeleteExpenseResponse
        {
            Message = "Expense deleted succesfully"
        };
    }
}
using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;

public interface IDeleteExpenseService
{
    Task<DeleteExpenseResponse> DeleteExpenseAsync(DeleteExpenseRequest request);
}
public class DeleteExpenseService : IDeleteExpenseService
{
    private readonly IDeleteExpenseRepository _deleteExpenseRepository;
    public DeleteExpenseService(IDeleteExpenseRepository deleteExpenseRepositor)
    {
        _deleteExpenseRepository = deleteExpenseRepositor;
    }
    public async Task<DeleteExpenseResponse> DeleteExpenseAsync(DeleteExpenseRequest request)
    {
        var deletedExpense = await _deleteExpenseRepository.DeleteExpenseAsync(request);
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
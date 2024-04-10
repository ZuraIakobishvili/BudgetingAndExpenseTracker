using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;

public interface IDeleteIncomeService
{
    Task<DeleteIncomeResponse> DeleteIncomeAsync(DeleteIncomeRequest request);
}
public class DeleteIncomeService : IDeleteIncomeService
{
    private readonly IDeleteIncomeRepository _deleteIncomeRepository;
    public DeleteIncomeService(IDeleteIncomeRepository deleteIncomeRepository)
    {
        _deleteIncomeRepository = deleteIncomeRepository;
    }

    public async Task<DeleteIncomeResponse> DeleteIncomeAsync(DeleteIncomeRequest request)
    {
        var deletedIncome = await _deleteIncomeRepository.DeleteIncomeAsync(request);

        if (!deletedIncome)
        {
            throw new InvalidIncomeException("Income can not be deleted.");
        }

        return new DeleteIncomeResponse
        {

            Message ="Income deleted succesfully"
        };
    }
}

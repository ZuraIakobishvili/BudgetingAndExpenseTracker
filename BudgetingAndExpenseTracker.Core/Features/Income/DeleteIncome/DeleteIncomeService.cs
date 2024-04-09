using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;

public interface IDeleteIncomeService
{
    Task<DeleteIncomeResponse> DeleteIncome(DeleteIncomeRequest request);
}
public class DeleteIncomeService : IDeleteIncomeService
{
    private readonly IDeleteIncomeRepository _deleteIncomeRepository;
    public DeleteIncomeService(IDeleteIncomeRepository deleteIncomeRepository)
    {
        _deleteIncomeRepository = deleteIncomeRepository;
    }

    public async Task<DeleteIncomeResponse> DeleteIncome(DeleteIncomeRequest request)
    {
        var deletedIncome = await _deleteIncomeRepository.DeleteIncome(request);

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

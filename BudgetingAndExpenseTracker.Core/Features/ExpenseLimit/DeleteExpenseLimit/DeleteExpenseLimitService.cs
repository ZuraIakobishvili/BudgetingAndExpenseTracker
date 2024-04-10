using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.DeleteExpenseLimit;

public interface IDeleteExpenseLimitService
{
    Task<DeleteExpenseLimitResponse> DeleteLimitAsync(DeleteExpenseLimitRequest request);
}

public class DeleteExpenseLimitService : IDeleteExpenseLimitService
{
    private readonly IDeleteExpenseLimitRepository _deleteExpenseLimitRepository;
    public DeleteExpenseLimitService(IDeleteExpenseLimitRepository deleteExpenseLimitRepository)
    {
        _deleteExpenseLimitRepository = deleteExpenseLimitRepository;
    }
    public async Task<DeleteExpenseLimitResponse> DeleteLimitAsync(DeleteExpenseLimitRequest request)
    {
        var deletedLimit = await _deleteExpenseLimitRepository.DeleteLimitAsync(request);
        if (!deletedLimit)
        {
            throw new InvalidLimitException("Limit can not be deleted");
        }

        return new DeleteExpenseLimitResponse
        {
            Message = "Limit deleted successfully"
        };
    }
}

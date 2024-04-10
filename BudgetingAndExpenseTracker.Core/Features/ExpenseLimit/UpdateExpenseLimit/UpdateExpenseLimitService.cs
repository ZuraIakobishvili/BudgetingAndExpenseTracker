using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;

public interface IUpdateExpenseLimitService
{
    Task<UpdateExpenseLimitResponse> UpdateExpenseLimitAsync(UpdateExpenseLimitRequest request);
}
public class UpdateExpenseLimitService : IUpdateExpenseLimitService
{
    private readonly IUpdateExpenseLimitRepository _updateExpenseLimitRepository;
    public UpdateExpenseLimitService(IUpdateExpenseLimitRepository updateExpenseLimitRepository)
    {
        _updateExpenseLimitRepository = updateExpenseLimitRepository;
    }
    public async Task<UpdateExpenseLimitResponse> UpdateExpenseLimitAsync(UpdateExpenseLimitRequest request)
    {
        ValidateUpdateLimitRequest(request);
        var updatedLimit = await _updateExpenseLimitRepository.UpdateLimitAsync(request);
        if (!updatedLimit)
        {
            throw new Exception("Limit did not updated");
        }

        return new UpdateExpenseLimitResponse
        {
            Message = "Limit updated successfully",
            Amount = request.Amount,
            Category = request.Category,
            Currency = request.Currency
        };
    }

    private void ValidateUpdateLimitRequest(UpdateExpenseLimitRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Update expense limit request cannot be null.");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidRequestException("Limit amount can not be zero or negative, try again.");
        }

        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Limit category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Limit currency is not valid.");
        }

        if (!Enum.IsDefined(typeof(Period), request.Period))
        {
            throw new InvalidRequestException("Limit period is not valid.");
        }
    }
}

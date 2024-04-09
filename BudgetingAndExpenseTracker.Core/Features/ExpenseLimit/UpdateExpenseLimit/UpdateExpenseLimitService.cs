namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;

public interface IUpdateExpenseLimitService
{
    Task<UpdateExpenseLimitResponse> UpdateExpenseLimit(UpdateExpenseLimitRequest request);
}
public class UpdateExpenseLimitService : IUpdateExpenseLimitService
{
    private readonly IUpdateExpenseLimitRepository _updateExpenseLimitRepository;
    public UpdateExpenseLimitService(IUpdateExpenseLimitRepository updateExpenseLimitRepository)
    {
        _updateExpenseLimitRepository = updateExpenseLimitRepository;
    }
    public async Task<UpdateExpenseLimitResponse> UpdateExpenseLimit(UpdateExpenseLimitRequest request)
    {
        UpdateLimitValidation.LimitValidation(request);
        var updatedLimit = await _updateExpenseLimitRepository.Update(request);
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
}

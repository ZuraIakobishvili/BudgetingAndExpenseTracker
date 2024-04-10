using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
public interface ICreateExpenseLimitService
{
    Task<CreateExpenseLimitResponse> CreateLimitAsync(CreateExpenseLimitRequest request);
}
public class CreateExpenseLimitService : ICreateExpenseLimitService
{
    private readonly ICreateExpenseLimitRepository _createExpenseLimitRepository;
    public CreateExpenseLimitService(ICreateExpenseLimitRepository createExpenseLimitRepository)
    {
        _createExpenseLimitRepository = createExpenseLimitRepository;
    }
    public async Task<CreateExpenseLimitResponse> CreateLimitAsync(CreateExpenseLimitRequest request)
    {
        ValidateLimitRequest(request);
        var createdExpenseLimit = await _createExpenseLimitRepository.CreateExpenseLimitAsync(request);
        if (!createdExpenseLimit)
        {
            throw new InvalidLimitException("Limit can not be created");
        }

        return new CreateExpenseLimitResponse
        {
            Message = "Limit created successfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category,
            LimitPeriod = request.LimitPeriod
        };
    }

    public void ValidateLimitRequest(CreateExpenseLimitRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Create expense limit request cannot be null.");
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

        if (!Enum.IsDefined(typeof(Period), request.LimitPeriod))
        {
            throw new InvalidRequestException("Limit period is not valid.");
        }
    }
}

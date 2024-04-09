using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
public interface ICreateExpenseLimitService
{
    Task<CreateExpenseLimitResponse> Create(CreateExpenseLimitRequest request);
}
public class CreateExpenseLimitService : ICreateExpenseLimitService
{
    private readonly ICreateExpenseLimitRepository _createExpenseLimitRepository;
    public CreateExpenseLimitService(ICreateExpenseLimitRepository createExpenseLimitRepository)
    {
        _createExpenseLimitRepository = createExpenseLimitRepository;
    }
    public async Task<CreateExpenseLimitResponse> Create(CreateExpenseLimitRequest request)
    {
        CreateExpenseLimitValidation.LimitValidation(request);
        var createdExpenseLimit = await _createExpenseLimitRepository.CreateExpenseLimit(request);
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
}

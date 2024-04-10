using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.GetExpenseLimits;

public interface IGetAllExpenseLimitsService
{
    Task<List<Entities.ExpenseLimit>> GetLimitsAsync(GetAllExpenseLimitsRequest request);
}

public class GetAllExpenseLimitsService : IGetAllExpenseLimitsService
{
    private readonly IGetAllExpenseLimitsRepository _getAllExpenseLimitsRepository;
    public GetAllExpenseLimitsService(IGetAllExpenseLimitsRepository getAllExpenseLimitsRepository)
    {
        _getAllExpenseLimitsRepository = getAllExpenseLimitsRepository;
    }
    public async Task<List<Entities.ExpenseLimit>> GetLimitsAsync(GetAllExpenseLimitsRequest request)
    {
        var limits = await _getAllExpenseLimitsRepository.GetExpenseLimitsAsync(request);
        if(limits.Count == 0)
        {
            throw new InvalidLimitException("Limits List is empty");
        }

        return limits;
    }
}

using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;

public interface IUpdateIncomeService
{
    Task<UpdateIncomeResponse> UpdateIncomeAsync(UpdateIncomeRequest request);
}
public class UpdateIncomeService : IUpdateIncomeService
{
    private readonly IUpdateIncomeRepository _updateIncomeRepository;
    public UpdateIncomeService(IUpdateIncomeRepository updateIncomeRepository)
    {
        _updateIncomeRepository = updateIncomeRepository;
    }
    public async Task<UpdateIncomeResponse> UpdateIncomeAsync(UpdateIncomeRequest request)
    {
        ValidateUpdateIncomeRequest(request);
        var updatedIncome = await _updateIncomeRepository.UpdateIncomeAsync(request);
        if (!updatedIncome)
        {
            throw new InvalidIncomeException("Income can not be updated");
        }

        return new UpdateIncomeResponse
        {
            Message = "Income updated succesfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category
        };
    }

    private void ValidateUpdateIncomeRequest(UpdateIncomeRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new InvalidRequestException("Income amount can not be zero or negative, try again.");
        }

        if (!Enum.IsDefined(typeof(IncomeCategory), request.Category))
        {
            throw new InvalidRequestException("Invalid income category.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Invalid income currency.");
        }
    }
}

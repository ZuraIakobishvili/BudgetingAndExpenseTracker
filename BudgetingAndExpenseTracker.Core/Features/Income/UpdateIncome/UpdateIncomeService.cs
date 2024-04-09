using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncome;

namespace BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;

public interface IUpdateIncomeService
{
    Task<UpdateIncomeResponse> UpdateIncome(UpdateIncomeRequest request);
}
public class UpdateIncomeService : IUpdateIncomeService
{
    private readonly IUpdateIncomeRepository _updateIncomeRepository;
    public UpdateIncomeService(IUpdateIncomeRepository updateIncomeRepository)
    {
        _updateIncomeRepository = updateIncomeRepository;
    }
    public async Task<UpdateIncomeResponse> UpdateIncome(UpdateIncomeRequest request)
    {
        UpdateIncomeValidation.IncomeValidation(request);
        var updatedIncome = await _updateIncomeRepository.Update(request);
        if(!updatedIncome)
        {
            throw new  InvalidIncomeException("Income can not be updated");
        }

        return new UpdateIncomeResponse
        {
            Message = "Income updated succesfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category
        };
    }
}

using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
public interface ICreateIncomeService
{
    Task<CreateIncomeResponse> CreateIncomeAsync(CreateIncomeRequest request);
}
public class CreateIncomeService : ICreateIncomeService
{
    private readonly ICreateIncomeRepository _createIncomeRepository;
    public CreateIncomeService(ICreateIncomeRepository createIncomeRepository)
    {
        _createIncomeRepository = createIncomeRepository;
    }
    public async Task<CreateIncomeResponse> CreateIncomeAsync(CreateIncomeRequest request)
    {
        ValidateCreateIncomeRequest(request);
        var createdIncome = await _createIncomeRepository.CreateIncomeAsync(request);

        if (!createdIncome)
        {
            throw new InvalidIncomeException("Income can not be created");
        }
        return new CreateIncomeResponse
        {
            Message = "Income created successfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category
        };
    }

    private void ValidateCreateIncomeRequest(CreateIncomeRequest request)
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

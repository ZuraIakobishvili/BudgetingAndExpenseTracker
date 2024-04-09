using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
public interface ICreateIncomeService
{
    Task<CreateIncomeResponse> Create(CreateIncomeRequest request);
}
public class CreateIncomeService : ICreateIncomeService
{
    private readonly ICreateIncomeRepository _createIncomeRepository;
    public CreateIncomeService(ICreateIncomeRepository createIncomeRepository)
    {
        _createIncomeRepository = createIncomeRepository;
    }
    public async Task<CreateIncomeResponse> Create(CreateIncomeRequest request)
    {
        CreateIncomeValidation.IncomeValidation(request);
        var createdIncome = await _createIncomeRepository.Create(request);

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
}

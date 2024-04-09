
using BudgetingAndExpenseTracker.Core.Exceptions;

namespace BudgetingAndExpenseTracker.Core.Features.Income.GetIncomes;

public interface IGetAllIncomesService
{
    Task<List<Entities.Income>> GetAllIncomes(GetAllIncomesRequest request);
}
public class GetAllIncomesService : IGetAllIncomesService
{
    private readonly IGetAllIncomesRepository _getAllIncomesRepository;
    public GetAllIncomesService(IGetAllIncomesRepository getAllIncomesRepository)
    {
       _getAllIncomesRepository = getAllIncomesRepository;
    }

    public async Task<List<Entities.Income>> GetAllIncomes(GetAllIncomesRequest request)
    {
        var incomes =  await _getAllIncomesRepository.GetIncomes(request);
        if(incomes.Count == 0)
        {
            throw new InvalidIncomeException("Incomes list is empty");
        }
        return incomes;
    }
}

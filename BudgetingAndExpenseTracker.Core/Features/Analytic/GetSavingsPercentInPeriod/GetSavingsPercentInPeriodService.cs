namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetSavingsPercentInPeriod;

public interface IGetSavingsPercentInPeriodService
{
    Task<GetSavingsPercentInPeriodResponse> GetSavingsPercentInPeriod(GetSavingsPercentInPeriodRequest request);
}
public class GetSavingsPercentInPeriodService : IGetSavingsPercentInPeriodService
{
    private readonly IGetSavingsPercentInPeriodRepository _getSavingsPercentInPeriodRepository;
    public GetSavingsPercentInPeriodService(IGetSavingsPercentInPeriodRepository getSavingsPercentInPeriodRepository)
    {
        _getSavingsPercentInPeriodRepository = getSavingsPercentInPeriodRepository;
    }

    public async Task<GetSavingsPercentInPeriodResponse> GetSavingsPercentInPeriod(GetSavingsPercentInPeriodRequest request)
    {
        var percent =  await _getSavingsPercentInPeriodRepository.GetSavingsPercentInPeriod(request);
       
        return new GetSavingsPercentInPeriodResponse
        {
            Message = "Savings in percent: ",
            Percent = percent,
            Currency = request.Currency,
            Period = request.Period
        };
    }
}

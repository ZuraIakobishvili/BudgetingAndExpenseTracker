namespace BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;

public interface IGetTotalSavingsInPeriodService
{
    Task<GetTotalSavingsInPeriodResponse> GetTotalSavingsInPeriodAsync(GetTotalSavingsInPeriodRequest request);
}
public class GetTotalSavingsInPeriodService : IGetTotalSavingsInPeriodService
{
    private readonly IGetTotalSavingsInPeriodRepository _getTotalSavingsInPeriodRepository;
    public GetTotalSavingsInPeriodService(IGetTotalSavingsInPeriodRepository getTotalSavingsInPeriodRepository)
    {
        _getTotalSavingsInPeriodRepository = getTotalSavingsInPeriodRepository;
    }

    public async Task<GetTotalSavingsInPeriodResponse> GetTotalSavingsInPeriodAsync(GetTotalSavingsInPeriodRequest request)
    {
        var savings  = await _getTotalSavingsInPeriodRepository.GetTotalSavingsAsync(request);
        return new GetTotalSavingsInPeriodResponse
        {
            Message = "Total savings:",
            Amount = savings,
            Currency = request.Currency,
            Period = request.Period,
        };
    }
}

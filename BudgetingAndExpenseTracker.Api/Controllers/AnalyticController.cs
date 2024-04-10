using BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;
using BudgetingAndExpenseTracker.Core.Features.Analytic.GetSavingsPercentInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;

[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/analytic")]

public class AnalyticController : ControllerBase
{
    private readonly IGetTotalSavingsInPeriodService _getTotalSavingsInPeriodService;
    private readonly IGetSavingsPercentInPeriodService _getSavingsPercentInPeriodService;
    private readonly IExpenseForecastService _expenseForecastService;
    private readonly IIncomeForecastService _incomeForecastService;
    public AnalyticController(
        IGetTotalSavingsInPeriodService getTotalSavingsInPeriodService,
        IGetSavingsPercentInPeriodService getSavingsPercentInPeriodService,
        IExpenseForecastService expenseForecastService,
        IIncomeForecastService incomeForecastService)
    {
        _getTotalSavingsInPeriodService = getTotalSavingsInPeriodService;
        _getSavingsPercentInPeriodService = getSavingsPercentInPeriodService;
        _expenseForecastService = expenseForecastService;
        _incomeForecastService = incomeForecastService;
    }

    [HttpPost]
    [Route("get-total-savings-in-period")]
    public async Task<IActionResult> GetTotalSaving(GetTotalSavingsInPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getTotalSavingsInPeriodService.GetTotalSavingsInPeriodAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-savings-percent-in-period")]
    public async Task<IActionResult> GetSavingPercent(GetSavingsPercentInPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getSavingsPercentInPeriodService.GetSavingsPercentInPeriodAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("next-month-expense-forecast")]
    public async Task<IActionResult> GetNextMonthExpenseForecasrt(ExpenseForecastRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _expenseForecastService.GetNextMonthExpenseForecastAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("next-month-income-forecast")]
    public async Task<IActionResult> GetNextMonthIncomeForecast(IncomeForecastRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _incomeForecastService.GetNextMonthIncomeForecastAsync(request);
        return Ok(result);
    }
}

using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;

[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/expense-reports")]

public class ExpenseReportController : ControllerBase
{
    private readonly IGetExpensesByCategoryAndPeriodService _expensesByCategoryAndPeriodService;
    private readonly IGetExpensesByCurrencyAndPeriodService _expensesByCurrencyAndPeriodService;
    private readonly IGetExpensesByCategoryAndCurrencyInPeriodService _expensesByCategoryAndCurrencyInPeriodService;
    private readonly IGetTopExpensesByCurrencyInPeriodService _getTopExpensesByCurrencyInPeriodService;
    public ExpenseReportController(
        IGetExpensesByCategoryAndPeriodService getExpensesByCategoryAndPeriodService,
        IGetExpensesByCurrencyAndPeriodService getExpensesByCurrencyAndPeriodService,
        IGetExpensesByCategoryAndCurrencyInPeriodService expensesByCategoryAndCurrencyInPeriodService,
        IGetTopExpensesByCurrencyInPeriodService getTopExpensesByCurrencyInPeriodService)
    {
        _expensesByCategoryAndPeriodService = getExpensesByCategoryAndPeriodService;
        _expensesByCurrencyAndPeriodService = getExpensesByCurrencyAndPeriodService;
        _expensesByCategoryAndCurrencyInPeriodService = expensesByCategoryAndCurrencyInPeriodService;
        _getTopExpensesByCurrencyInPeriodService = getTopExpensesByCurrencyInPeriodService;
    }
    [HttpPost]
    [Route("get-expenses-by-period-and-category")]
    public async Task<IActionResult> GetExpensesByCategoryAndPeriod(GetExpensesByCategoryAndPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _expensesByCategoryAndPeriodService.GetExpensesByCategoryAndPeriod(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-expenses-by-period-and-currency")]
    public async Task<IActionResult> GetExpensesByCurrencyAndPeriod(GetExpensesByCurrencyAndPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _expensesByCurrencyAndPeriodService.GetExpensesByCurrencyAndPeriod(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-expenses-by-category-and-currency-in-period")]
    public async Task<IActionResult> GetExpensesByCurrencyAndCategoryPeriod(GetExpensesByCategoryAndCurrencyInPeriodRequest request)
    {
        request.UserId= UserHelper.GetCurrentUserId(User);
        var result = await _expensesByCategoryAndCurrencyInPeriodService.GetExpensesByCategoryAndCurrencyInPeriod(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-top-expenses-by-currency-in-period")]
    public async Task<IActionResult> GetTopExpenses(GetTopExpensesByCurrencyInPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getTopExpensesByCurrencyInPeriodService.GetTopExpenses(request);
        return Ok(result);
    }
}

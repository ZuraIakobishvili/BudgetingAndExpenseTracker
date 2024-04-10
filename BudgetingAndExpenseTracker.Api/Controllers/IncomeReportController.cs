using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;

[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/expense-reports")]

public class IncomeReportController : ControllerBase
{
    private readonly IGetIncomesByCurrencyAndPeriodService _getIncomesByCurrencyAndPeriodService;
    private readonly IGetIncomesByCategoryAndPeriodService _getIncomesByCategoryAndPeriodService;
    private readonly IGetIncomesByCurrencyAndCategoryService _getIncomesByCurrencyAndCategoryService;
    public IncomeReportController(
        IGetIncomesByCurrencyAndPeriodService getIncomesByCurrencyAndPeriodService,
        IGetIncomesByCategoryAndPeriodService getIncomesByCategoryAndPeriodService,
        IGetIncomesByCurrencyAndCategoryService getIncomesByCurrencyAndCategoryService)
    {
        _getIncomesByCurrencyAndPeriodService = getIncomesByCurrencyAndPeriodService;
        _getIncomesByCategoryAndPeriodService = getIncomesByCategoryAndPeriodService;
        _getIncomesByCurrencyAndCategoryService = getIncomesByCurrencyAndCategoryService;
    }

    [HttpPost]
    [Route("get-incomes-by-currency-and-period")]
    public async Task<IActionResult> GetIncomesByCurrenctyAndPeriod(GetIncomesByCurrencyAndPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getIncomesByCurrencyAndPeriodService.GetIncomesByCurrencyAndPeriodAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-incomes-by-category-and-period")]
    public async Task<IActionResult> GetIncomesByCategoryAndPeriod(GetIncomesByCategoryAndPeriodRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getIncomesByCategoryAndPeriodService.GetIncomesByCategoryAndPeriodAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-incomes-by-category-and-currency-in-period")]
    public async Task<IActionResult> GetIncomesByCategoryAndCurrency(GetIncomesByCurrencyAndCategoryRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getIncomesByCurrencyAndCategoryService.GetIncomesByCategoryAndCurrencyInPeriodAsync(request);
        return Ok(result);
    }
}


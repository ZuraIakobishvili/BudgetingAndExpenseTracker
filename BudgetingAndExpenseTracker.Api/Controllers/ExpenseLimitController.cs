using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.DeleteExpenseLimit;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.GetExpenseLimits;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;

[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/limits")]

public class ExpenseLimitController : ControllerBase
{
    private readonly ICreateExpenseLimitService _createExpenseLimitService;
    private readonly IUpdateExpenseLimitService _updateExpenseLimitService;
    private readonly IDeleteExpenseLimitService _deleteExpenseLimitService;
    private readonly IGetAllExpenseLimitsService _getAllExpenseLimitsService;
    public ExpenseLimitController(
        ICreateExpenseLimitService createExpenseLimitService,
        IUpdateExpenseLimitService updateExpenseLimitService,
        IDeleteExpenseLimitService deleteExpenseLimitService,
        IGetAllExpenseLimitsService getAllExpenseLimitsService)
    {
        _createExpenseLimitService = createExpenseLimitService;
        _updateExpenseLimitService = updateExpenseLimitService;
        _deleteExpenseLimitService = deleteExpenseLimitService;
        _getAllExpenseLimitsService = getAllExpenseLimitsService;
    }

    [HttpPost]
    [Route("get-limits")]
    public async Task<IActionResult> GetLimits(GetAllExpenseLimitsRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result =  await _getAllExpenseLimitsService.GetLimits(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("create-limit")]
    public async Task<IActionResult> CreateLimit(CreateExpenseLimitRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _createExpenseLimitService.Create(request);
        return Ok(result);
    }

    [HttpPut]
    [Route("update-limit")]
    public async Task<IActionResult> UpdateLimit(UpdateExpenseLimitRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _updateExpenseLimitService.UpdateExpenseLimit(request);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete-limit")]
    public async Task<IActionResult> DeleteLimit(DeleteExpenseLimitRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _deleteExpenseLimitService.Delete(request);
        return Ok(result);
    }
}

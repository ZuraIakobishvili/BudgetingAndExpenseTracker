using BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;
using BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;
using BudgetingAndExpenseTracker.Core.Features.Expense.GetExpenses;
using BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;

[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/expenses")]

public class ExpenseController : ControllerBase
{
    private readonly ICreateExpenseService _createExpenseService;
    private readonly IUpdateExpenseService _updateExpenseService;
    private readonly IGetAllExpensesService _getAllExpensesService;
    private readonly IDeleteExpenseService _deleteExpenseService;
    public ExpenseController(
        ICreateExpenseService createExpenseService,
        IUpdateExpenseService updateExpenseService,
        IGetAllExpensesService getAllExpensesService,
        IDeleteExpenseService deleteExpenseService)
    {
        _createExpenseService = createExpenseService;
        _updateExpenseService = updateExpenseService;
        _getAllExpensesService = getAllExpensesService;
        _deleteExpenseService = deleteExpenseService;
    }

    [HttpPost]
    [Route("get-expenses")]
    public async Task<IActionResult> GetExpenses(GetAllExpensesRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getAllExpensesService.GetExpenses(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("create-expense")]
    public async Task<IActionResult> CreateExpense(CreateExpenseRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _createExpenseService.CreateExpense(request);
        return Ok(result);
    }

    [HttpPut]
    [Route("update-expense")]
    public async Task<IActionResult> UpdateExpense(UpdateExpenseRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _updateExpenseService.UpdateExpense(request);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete-expense")]
    public async Task<IActionResult> DeleteExpense(DeleteExpenseRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _deleteExpenseService.DeleteExpense(request);
        return Ok(result);
    }
}

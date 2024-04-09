using BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
using BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;
using BudgetingAndExpenseTracker.Core.Features.Income.GetIncomes;
using BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;
using BudgetingAndExpenseTracker.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BudgetingAndExpenseTracker.Api.Controllers;
[ApiController]
[Authorize("UserPolicy", AuthenticationSchemes = "Bearer")]
[Route("api/income")]
public class IncomeController : ControllerBase
{
    private readonly ICreateIncomeService _createIncomeService;
    private readonly IUpdateIncomeService _updateIncomeService;
    private readonly IDeleteIncomeService _deleteIncomeService;
    private readonly IGetAllIncomesService _getAllIncomesService;
    public IncomeController(
        ICreateIncomeService createIncomeService,
        IUpdateIncomeService updateIncomeService,
        IDeleteIncomeService deleteIncomeService,
        IGetAllIncomesService getAllIncomesService)
    {
        _createIncomeService = createIncomeService;
        _updateIncomeService = updateIncomeService;
        _deleteIncomeService = deleteIncomeService;
        _getAllIncomesService = getAllIncomesService;
    }

    [HttpPost]
    [Route("get-incomes")]
    public async Task<IActionResult> GetIncomes(GetAllIncomesRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _getAllIncomesService.GetAllIncomes(request);   
        return Ok(result);
    }

    [HttpPost]
    [Route("create-income")]
    public async Task<IActionResult> Create(CreateIncomeRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result  =  await _createIncomeService.Create(request);
        return Ok(result);
    }

    [HttpPut]
    [Route("update-income")]
    public async Task<IActionResult> Update(UpdateIncomeRequest request)
    {
        request.UserId = UserHelper.GetCurrentUserId(User);
        var result = await _updateIncomeService.UpdateIncome(request);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete-income")]
    public async Task<IActionResult> Delete(DeleteIncomeRequest request)
    {
        request.UserId= UserHelper.GetCurrentUserId(User);
        var result = await _deleteIncomeService.DeleteIncome(request);
        return Ok(result);
    }
}

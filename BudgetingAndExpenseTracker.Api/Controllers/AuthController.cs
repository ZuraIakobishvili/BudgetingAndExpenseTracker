using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Features.Account.Login;
using BudgetingAndExpenseTracker.Core.Features.Account.Registration;
using BudgetingAndExpenseTracker.Core.Services.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingAndExpenseTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterUserService _registerUserService;
    private readonly ILoginService _loginService;
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
        IRegisterUserService registerUserService,
        ILoginService loginService,
        UserManager<AppUser> userManager,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _registerUserService = registerUserService;
        _loginService = loginService;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var result = await _registerUserService.RegisterUserAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        var result = await _loginService.LoginAsync(request);
        var jwt = _jwtTokenGenerator.Generate(user!.Id.ToString(), request.Email);

        return Ok(new { result, Token = jwt });
    }
}

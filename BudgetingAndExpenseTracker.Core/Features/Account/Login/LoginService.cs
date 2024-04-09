using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BudgetingAndExpenseTracker.Core.Features.Account.Login;

public interface ILoginService
{
    Task<LoginResponse> Login(LoginRequest request);
}
public class LoginService : ILoginService
{
    private readonly UserManager<AppUser> _userManager;
    public LoginService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        ValidateLoginRequest(request);

         var user = await _userManager.FindByEmailAsync(request.Email);

        
        if (user == null)
        {
            throw new UserNotFoundException("User does not exist...");
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isCorrectPassword)
        {
            throw new InvalidRequestException("Email or password is incorrect");
        }

        return new LoginResponse
        {
            Message = "Login succesfully",
            Username = $"Hello, {user.FirstName}"
        };
    }

    private void ValidateLoginRequest(LoginRequest request)
    {
        if(request == null)
        {
            throw new ArgumentException(nameof(request));
        }
        if (string.IsNullOrEmpty(request.Email))
        {
            throw new InvalidRequestException("Email can not be null...");
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            throw new InvalidRequestException("Password can not be null...");
        }

    }
}

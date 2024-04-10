using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace BudgetingAndExpenseTracker.Core.Features.Account.Registration;

public interface IRegisterUserService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
}
public class RegisterUserService : IRegisterUserService
{
    private readonly UserManager<AppUser> _userManager;
    public RegisterUserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {

        RegisterUserValidation(request);
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            RegisterDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.FirstOrDefault()?.Description);
            throw new Exception($"Registration failed: {error}");
        }

        return new RegisterUserResponse
        {
            Message = "User registered successfully",
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };
    }

    private void RegisterUserValidation(RegisterUserRequest request)
    {
        if (request == null)
        {
            throw new ArgumentException("Request can not be null");
        }
            string pattern = @"^[A-Z][a-zA-Z]+$";
        if (!Regex.IsMatch(request.FirstName, pattern))
        {
            throw new InvalidRequestException("First name should start with an uppercase letter and contain only Latin letters.");
        }

        if (!Regex.IsMatch(request.LastName, pattern))
        {
            throw new InvalidRequestException("Last name should start with an uppercase letter and contain only Latin letters.");
        }
    }
}


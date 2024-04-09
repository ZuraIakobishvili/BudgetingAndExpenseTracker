namespace BudgetingAndExpenseTracker.Core.Features.Account.Login;
public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}


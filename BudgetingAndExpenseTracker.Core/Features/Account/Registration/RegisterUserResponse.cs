namespace BudgetingAndExpenseTracker.Core.Features.Account.Registration;
public class RegisterUserResponse
{
    public required string Message { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

using BudgetingAndExpenseTracker.Core.Features.Account.Login;
namespace BudgetingAndExpenseTracker.Tests.Factory;
public class RequestFactory
{
    public static LoginRequest GetLoginRequest(Action<LoginRequest> options = null)
    {
        LoginRequest request = new()
        {
            Email = "test@gmail.com",
            Password = "Password1234"
        };

        options?.Invoke(request);

        return request;
    }
}

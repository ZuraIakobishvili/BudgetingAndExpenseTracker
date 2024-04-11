using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Features.Account.Login;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace BudgetingAndExpenseTracker.Tests.Features.Account.AccountLogin;

public class AccountLoginTest
{
    private ILoginService _loginService;
    private UserManager<AppUser> _fakeUserManager;
    [SetUp]
    public void Setup()
    {
        _fakeUserManager = A.Fake<UserManager<AppUser>>();
        _loginService = new LoginService(_fakeUserManager);
    }

    [Test]
    public void ShouldThrowInvalidRequestExceptionIfRequestIsNull()
    {
        Assert.ThrowsAsync<InvalidRequestException>(
                async () => await _loginService.LoginAsync(null)
            );
    }

}

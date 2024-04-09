using SendGrid.Helpers.Errors.Model;
using System.Security.Claims;

namespace BudgetingAndExpenseTracker.Core.Shared;

public class UserHelper
{
    public static string GetCurrentUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            throw new NotFoundException("User does not exists.");
        }
        return userId!;
    }

    public static DateTime GetStartDay(Period period)
    {
        DateTime startDate;
        if (period == Period.Month)
        {
            startDate = DateTime.Now.AddDays(-30);
        }
        else if (period == Period.Quarter)
        {
            startDate = DateTime.Now.AddDays(-90);
        }
        else if (period == Period.Year)
        {
            startDate = DateTime.Now.AddDays(-365);
        }
        else
        {
            throw new Exception("Invalid period specified");
        }

        return startDate;
    }
}

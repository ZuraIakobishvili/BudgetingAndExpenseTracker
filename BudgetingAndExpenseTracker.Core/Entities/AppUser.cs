using Microsoft.AspNetCore.Identity;

namespace BudgetingAndExpenseTracker.Core.Entities;
public class AppUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime RegisterDate { get; set; }
}

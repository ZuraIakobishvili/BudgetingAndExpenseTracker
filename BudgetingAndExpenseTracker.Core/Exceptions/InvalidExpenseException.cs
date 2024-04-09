namespace BudgetingAndExpenseTracker.Core.Exceptions;
public class InvalidExpenseException : Exception
{
    public InvalidExpenseException(string message) : base(message)
    {
        
    }
}

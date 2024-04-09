namespace BudgetingAndExpenseTracker.Core.Exceptions;

public  class InvalidLimitException : Exception
{
    public InvalidLimitException(string message) : base(message)
    {
        
    }
}

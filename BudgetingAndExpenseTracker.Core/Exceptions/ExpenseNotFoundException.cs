namespace BudgetingAndExpenseTracker.Core.Exceptions;

public class ExpenseNotFoundException : Exception
{
    public ExpenseNotFoundException(string message) : base(message)
    {
        
    }
}

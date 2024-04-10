using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;

public interface IUpdateExpenseService
{
    Task<UpdateExpenseResponse> UpdateExpenseAsync(UpdateExpenseRequest request);
}

public class UpdateExpenseService : IUpdateExpenseService
{
    private readonly IUpdateExpenseRepository _updateExpenseRepository;
    private readonly IDbConnection _dbConnection;

    public UpdateExpenseService(
        IUpdateExpenseRepository updateExpenseRepository,
        IDbConnection dbConnection)
    {
        _updateExpenseRepository = updateExpenseRepository;
        _dbConnection = dbConnection;
    }

    public async Task<UpdateExpenseResponse> UpdateExpenseAsync(UpdateExpenseRequest request)
    {
        var monthLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Month);
        var quarterLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Quarter);
        var yearLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Year);

        var monthStartDate = UserHelper.GetStartDay(Period.Month);
        var quarterStartDate = UserHelper.GetStartDay(Period.Quarter);
        var yearStartDate = UserHelper.GetStartDay(Period.Year);

        var monthlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, monthStartDate);
        var quarterlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, quarterStartDate);
        var yearlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, yearStartDate);

        await ValidateExpenseLimitAsync(request, Period.Month, monthLimit, monthlyExpenses);
        await ValidateExpenseLimitAsync(request, Period.Quarter, quarterLimit, quarterlyExpenses);
        await ValidateExpenseLimitAsync(request, Period.Year, yearLimit, yearlyExpenses);

        var updatedExpense = await _updateExpenseRepository.UpdateExpenseAsync(request);
        if (!updatedExpense)
        {
            throw new InvalidExpenseException("Expense can not be updated");
        }

        return new UpdateExpenseResponse
        {
            Message = "Expense was updated successfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category
        };
    }

    private async Task<decimal> GetExpensesByCurrencyAndCategoryInPeriodAsync(string userId, ExpenseCategory category, Currency currency, DateTime limitPeriodStartDate)
    {
        var query = "SELECT SUM(Amount) FROM Expenses WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND ExpenseDate >= @StartDate AND ExpenseDate <= @EndDate";
        var endDate = DateTime.Now;
        var totalExpense = await _dbConnection.QueryFirstOrDefaultAsync<decimal?>(query, new { UserId = userId, Category = category, Currency = currency, StartDate = limitPeriodStartDate, EndDate = endDate }) ?? 0;
        return totalExpense;
    }

    private async Task<List<Entities.Income>> GetIncomesAsync(string userId)
    {
        var query = "SELECT * FROM Incomes WHERE UserId = @UserId";
        return (await _dbConnection.QueryAsync<Entities.Income>(query, new { UserId = userId })).ToList();
    }

    private async Task<decimal> GetTotalIncomeAmountInCurrencyAsync(string userId, Currency currency)
    {
        var incomes = await GetIncomesAsync(userId);

        var totalIncomeSumInCurrency = incomes
            .Where(income => income.Currency == currency)
            .Sum(x => x.Amount);
        return totalIncomeSumInCurrency;
    }


    private async Task<Entities.ExpenseLimit> GetLimitInPeriodAsync(string userId, ExpenseCategory category, Currency currency, Period period)
    {
        var query = "SELECT * FROM Limits WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND LimitPeriod = @Period";
        var limit = await _dbConnection.QueryFirstOrDefaultAsync<Entities.ExpenseLimit>(query, new { UserId = userId, Category = category, Currency = currency, Period = period });

        if (limit == null)
        {
            throw new NotFoundException("You should add limits for all periods in this category and currency, before updating expense.");
        }
        return limit;
    }

    private async Task ValidateExpenseLimitAsync(UpdateExpenseRequest request, Period period, Entities.ExpenseLimit limit, decimal periodExpenses)
    {
        if (limit == null)
        {
            throw new NotFoundException($"Expense limit for category '{request.Category}' in currency '{request.Currency}' for {period} not found.");
        }

        if (await IsPeriodExpenseLimitExceededAsync(request, limit, periodExpenses))
        {
            throw new InvalidRequestException($"Updated expense by category '{request.Category}' in currency '{request.Currency}' exceeds the {period} limit or total income.");
        }
    }

    private async Task<bool> IsPeriodExpenseLimitExceededAsync(UpdateExpenseRequest request, Entities.ExpenseLimit limitPeriod, decimal periodExpenses)
    {
        if (limitPeriod == null)
        {
            return false;
        }

        var totalIncome = await GetTotalIncomeAmountInCurrencyAsync(request.UserId, request.Currency);

        return periodExpenses + request.Amount > limitPeriod.Amount || periodExpenses + request.Amount > totalIncome;
    }
}
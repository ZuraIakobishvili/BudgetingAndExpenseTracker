﻿using BudgetingAndExpenseTracker.Core.Exceptions;
using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;

public interface ICreateExpenseService
{
    Task<CreateExpenseResponse> CreateExpenseAsync(CreateExpenseRequest request);
}

public class CreateExpenseService : ICreateExpenseService
{
    private readonly ICreateExpenseRepository _createExpenseRepository;
    private readonly IDbConnection _dbConnection;

    public CreateExpenseService(
        ICreateExpenseRepository createExpenseRepository,
        IDbConnection dbConnection)
    {
        _createExpenseRepository = createExpenseRepository;
        _dbConnection = dbConnection;
    }

    public async Task<CreateExpenseResponse> CreateExpenseAsync(CreateExpenseRequest request)
    {
        ExpenseRequestValidation(request);

        var monthLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Month);
        var quarterLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Quarter);
        var yearLimit = await GetLimitInPeriodAsync(request.UserId, request.Category, request.Currency, Period.Year);

        var monthStartDate = UserHelper.GetStartDay(Period.Month);
        var quarterStartDate = UserHelper.GetStartDay(Period.Quarter);
        var yearStartDate = UserHelper.GetStartDay(Period.Year);

        var monthlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, monthStartDate);
        var quarterlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, quarterStartDate);
        var yearlyExpenses = await GetExpensesByCurrencyAndCategoryInPeriodAsync(request.UserId, request.Category, request.Currency, yearStartDate);


        if (await IsPeriodExpenseLimitExceededAsync(request, monthLimit, monthlyExpenses))
        {
            throw new InvalidRequestException($"Expense by category '{request.Category}' in currency '{request.Currency}' exceeds the {Period.Month} limit.");
        }

        if (await IsPeriodExpenseLimitExceededAsync(request, quarterLimit, quarterlyExpenses))
        {
            throw new InvalidRequestException($"Expense by category '{request.Category}' in currency '{request.Currency}' exceeds the {Period.Quarter} limit.");
        }

        if (await IsPeriodExpenseLimitExceededAsync(request, yearLimit, yearlyExpenses))
        {
            throw new InvalidRequestException($"Expense by category '{request.Category}' in currency '{request.Currency}' exceeds the {Period.Year} limit.");
        }

        var  createdExpense = await _createExpenseRepository.CreateExpenseAsync(request);
        if (!createdExpense)
        {
            throw new InvalidExpenseException("Expense can not be created.");
        }

        return new CreateExpenseResponse
        {
            Message = "Expense created successfully",
            Amount = request.Amount,
            Currency = request.Currency,
            Category = request.Category
        };
    }

    public async Task<decimal> GetExpensesByCurrencyAndCategoryInPeriodAsync(string userId, ExpenseCategory category, Currency currency, DateTime limitPeriodStartDate)
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

    public async Task<decimal> GetTotalIncomeAmountInCurrencyAsync(string userId, Currency currency)
    {
        var incomes = await GetIncomesAsync(userId);

        var totalIncomeSumInCurrency = incomes
            .Where(income => income.Currency == currency)
            .Sum(x => x.Amount);
        return totalIncomeSumInCurrency;
    }

    public async Task<Entities.ExpenseLimit> GetLimitInPeriodAsync(string userId, ExpenseCategory category, Currency currency, Period period)
    {
        var query = "SELECT * FROM Limits WHERE UserId = @UserId AND Category = @Category AND Currency = @Currency AND LimitPeriod = @Period";
        var limit = await _dbConnection.QueryFirstOrDefaultAsync<Entities.ExpenseLimit>(query, new { UserId = userId, Category = category, Currency = currency, Period = period });

        if (limit == null)
        {
            throw new NotFoundException("You should add limits for all periods in this category and currency, before create expense.");
        }
        return limit;
    }

    private async Task<bool> IsPeriodExpenseLimitExceededAsync(CreateExpenseRequest request, Entities.ExpenseLimit limitPeriod, decimal periodExpenses)
    {
        if (limitPeriod == null)
        {
            return false;
        }

        var totalIncome = await GetTotalIncomeAmountInCurrencyAsync(request.UserId, request.Currency);
        var totalExpenses = periodExpenses + request.Amount;

        if (totalExpenses > totalIncome)
        {
            throw new InvalidExpenseException($"Please add income in this currency.");
        }

        return totalExpenses > limitPeriod.Amount;
    }

    private void ExpenseRequestValidation(CreateExpenseRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Create expense request cannot be null.");
        }

        if (request.Amount <= 0)
        {
            throw new InvalidRequestException("Limit amount can not be zero or negative, try again.");
        }

        if (!Enum.IsDefined(typeof(ExpenseCategory), request.Category))
        {
            throw new InvalidRequestException("Limit category is not valid.");
        }

        if (!Enum.IsDefined(typeof(Currency), request.Currency))
        {
            throw new InvalidRequestException("Limit currency is not valid.");
        }
    }
}
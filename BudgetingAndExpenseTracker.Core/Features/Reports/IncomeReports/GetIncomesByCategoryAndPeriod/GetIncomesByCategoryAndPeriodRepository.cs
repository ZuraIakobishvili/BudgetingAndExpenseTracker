﻿using BudgetingAndExpenseTracker.Core.Shared;
using Dapper;
using System.Data;

namespace BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;

public interface IGetIncomesByCategoryAndPeriodRepository
{
    Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request);
}

public class GetIncomesByCategoryAndPeriodRepository : IGetIncomesByCategoryAndPeriodRepository
{
    private readonly IDbConnection _dbConnection;
    public GetIncomesByCategoryAndPeriodRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Entities.Income>> GetIncomesByCategoryAndPeriodAsync(GetIncomesByCategoryAndPeriodRequest request)
    {
        var startDate = UserHelper.GetStartDay(request.Period);
        var endDate = DateTime.Now;

        var query = @"
                SELECT * FROM Incomes
                WHERE UserId = @UserId
                    AND Category = @Category 
                    AND IncomeDate  >= @StartDate  
                    AND IncomeDate  <= @EndDate";

        var parameters = new
        {
            request.UserId,
            request.Category,
            StartDate = startDate,
            EndDate = endDate
        };

        return (await _dbConnection.QueryAsync<Entities.Income>(query, parameters)).ToList();
    }
}
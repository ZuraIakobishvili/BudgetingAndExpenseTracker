using BudgetingAndExpenseTracker.Api.Swagger;
using BudgetingAndExpenseTracker.Core.Entities;
using BudgetingAndExpenseTracker.Core.Features.Account.Login;
using BudgetingAndExpenseTracker.Core.Features.Account.Registration;
using BudgetingAndExpenseTracker.Core.Features.Analytic.ExpenseForecast;
using BudgetingAndExpenseTracker.Core.Features.Analytic.GetSavingsPercentInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Analytic.GetTotalSavingsInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Analytic.IncomeForecast;
using BudgetingAndExpenseTracker.Core.Features.Expense.CreateExpense;
using BudgetingAndExpenseTracker.Core.Features.Expense.DeleteExpense;
using BudgetingAndExpenseTracker.Core.Features.Expense.GetExpenses;
using BudgetingAndExpenseTracker.Core.Features.Expense.UpdateExpense;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.CerateExpenseLimit;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.DeleteExpenseLimit;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.GetExpenseLimits;
using BudgetingAndExpenseTracker.Core.Features.ExpenseLimit.UpdateExpenseLimit;
using BudgetingAndExpenseTracker.Core.Features.Income.CreateIncome;
using BudgetingAndExpenseTracker.Core.Features.Income.DeleteIncome;
using BudgetingAndExpenseTracker.Core.Features.Income.GetIncomes;
using BudgetingAndExpenseTracker.Core.Features.Income.UpdateIncomeFeature;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetExpensesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.ExpenseReports.GetTopExpensesByCurrencyInPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCategoryAndPeriod;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndCategory;
using BudgetingAndExpenseTracker.Core.Features.Reports.IncomeReports.GetIncomesByCurrencyAndPeriod;
using BudgetingAndExpenseTracker.Core.Services.DbContext;
using BudgetingAndExpenseTracker.Core.Services.Jwt;
using BudgetingAndExpenseTracker.Core.Services.Logger;
using BudgetingAndExpenseTracker.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace BudgetingAndExpenseTracker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var issuer = "myapp.com";

        var audience = "myapp.com";

        var secretKey = builder.Configuration["JwtTokenSecretKey"];

        builder.Services.Configure<JwtSettings>(s =>
        {
            s.Issuer = issuer;
            s.Audience = audience;
            s.SecretKey = secretKey;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };

        builder.Services.AddTransient<JwtTokenGenerator>();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("UserPolicy",
                    policy =>
                    {
                        policy.RequireClaim(ClaimTypes.Role, "api-user");
                    }
                );
        });


        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure connection string
        var username = Environment.UserName;
        var connectionString = builder.Configuration.GetConnectionString(username);
        builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));


        // Configure DbContext and Identity
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString(username)));

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationOptions>();

        builder.Services.AddScoped<ICreateIncomeRepository, CreateIncomeRepository>();
        builder.Services.AddScoped<ICreateIncomeService, CreateIncomeService>();
        builder.Services.AddScoped<IUpdateIncomeRepository, UpdateIncomeRepository>();
        builder.Services.AddScoped<IUpdateIncomeService, UpdateIncomeService>();
        builder.Services.AddScoped<IDeleteIncomeRepository, DeleteIncomeRepository>();
        builder.Services.AddScoped<IDeleteIncomeService, DeleteIncomeService>();
        builder.Services.AddScoped<IGetAllIncomesRepository, GetAllIncomesRepository>();
        builder.Services.AddScoped<IGetAllIncomesService, GetAllIncomesService>();

        builder.Services.AddScoped<ICreateExpenseLimitRepository, CreateExpenseLimitRepository>();
        builder.Services.AddScoped<ICreateExpenseLimitService, CreateExpenseLimitService>();
        builder.Services.AddScoped<IUpdateExpenseLimitRepository,  UpdateExpenseLimitRepository>();
        builder.Services.AddScoped<IUpdateExpenseLimitService,  UpdateExpenseLimitService>();
        builder.Services.AddScoped<IDeleteExpenseLimitRepository,  DeleteExpenseLimitRepository>();
        builder.Services.AddScoped<IDeleteExpenseLimitService,  DeleteExpenseLimitService>();
        builder.Services.AddScoped<IGetAllExpenseLimitsRepository, GetAllExpenseLimitsRepository>();
        builder.Services.AddScoped<IGetAllExpenseLimitsService, GetAllExpenseLimitsService>();

        builder.Services.AddScoped<ICreateExpenseRepository, CreateExpenseRepository>();
        builder.Services.AddScoped<ICreateExpenseService, CreateExpenseService>();
        builder.Services.AddScoped<IUpdateExpenseRepository, UpdateExpenseRepository>();
        builder.Services.AddScoped<IUpdateExpenseService, UpdateExpenseService>();
        builder.Services.AddScoped<IGetAllExpensesRepository,  GetAllExpensesRepository>();
        builder.Services.AddScoped<IGetAllExpensesService, GetAllExpensesService>();
        builder.Services.AddScoped<IDeleteExpenseRepository, DeleteExpenseRepository>();
        builder.Services.AddScoped<IDeleteExpenseService, DeleteExpenseService>();

        builder.Services.AddScoped<IGetExpensesByCategoryAndPeriodRepository, GetExpensesByCategoryAndPeriodRepository>();
        builder.Services.AddScoped<IGetExpensesByCategoryAndPeriodService, GetExpensesByCategoryAndPeriodService>();
        builder.Services.AddScoped<IGetExpensesByCurrencyAndPeriodRepository, GetExpensesByCurrencyAndPeriodRepository>();
        builder.Services.AddScoped<IGetExpensesByCurrencyAndPeriodService, GetExpensesByCurrencyAndPeriodService>();
        builder.Services.AddScoped<IGetExpensesByCategoryAndPeriodRepository, GetExpensesByCategoryAndPeriodRepository>();
        builder.Services.AddScoped<IGetExpensesByCategoryAndPeriodService, GetExpensesByCategoryAndPeriodService>();
        builder.Services.AddScoped<IGetTopExpensesByCurrencyInPeriodRepositoy, GetTopExpensesByCurrencyInPeriodRepositoy>();
        builder.Services.AddScoped<IGetTopExpensesByCurrencyInPeriodService, GetTopExpensesByCurrencyInPeriodService>();
        builder.Services.AddScoped<IGetExpensesByCategoryAndCurrencyInPeriodRepository, GetExpensesByCategoryAndCurrencyInPeriodRepository>();
        builder.Services.AddScoped<IGetExpensesByCategoryAndCurrencyInPeriodService, GetExpensesByCategoryAndCurrencyInPeriodService>();

        builder.Services.AddScoped<IGetIncomesByCurrencyAndPeriodRepository, GetIncomesByCurrencyAndPeriodRepository>();
        builder.Services.AddScoped<IGetIncomesByCurrencyAndPeriodService, GetIncomesByCurrencyAndPeriodService>();
        builder.Services.AddScoped<IGetIncomesByCategoryAndPeriodRepository, GetIncomesByCategoryAndPeriodRepository>();
        builder.Services.AddScoped<IGetIncomesByCategoryAndPeriodService, GetIncomesByCategoryAndPeriodService>();
        builder.Services.AddScoped<IGetIncomesByCurrencyAndCategoryRepository,  GetIncomesByCurrencyAndCategoryRepository>();
        builder.Services.AddScoped<IGetIncomesByCurrencyAndCategoryService,  GetIncomesByCurrencyAndCategoryService>();

        builder.Services.AddScoped<IGetTotalSavingsInPeriodRepository, GetTotalSavingsInPeriodRepository>();
        builder.Services.AddScoped<IGetTotalSavingsInPeriodService, GetTotalSavingsInPeriodService>();
        builder.Services.AddScoped<IGetSavingsPercentInPeriodRepository, GetSavingsPercentInPeriodRepository>();
        builder.Services.AddScoped<IGetSavingsPercentInPeriodService, GetSavingsPercentInPeriodService>();
        builder.Services.AddScoped<IExpenseForecastRepository, ExpenseForecastRepository>();
        builder.Services.AddScoped<IExpenseForecastService, ExpenseForecastService>();
        builder.Services.AddScoped<IIncomeForecastRepository, IncomeForecastRepository>();
        builder.Services.AddScoped<IIncomeForecastService, IncomeForecastService>();
        




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

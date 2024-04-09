using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace BudgetingAndExpenseTracker.Core.Services.Logger;
public interface ILoggerService
{
    void LogError(string message, params object?[]? parameters);
    void LogFatal(string message, params object?[]? parameters);
    void LogInformation(string message, params object?[]? parameters);
}

public class LoggerService : ILoggerService
{
    private readonly ILogger _errorLogger;
    private readonly ILogger _infoLogger;
    private readonly ILogger _fatalLogger;

    public LoggerService(IConfiguration configuration)
    {
        var logPath = configuration["Logging:FilePath"];
        var seqUrl = configuration["Logging:SeqUrl"];

        _errorLogger = SetupLogger(LogEventLevel.Error, logPath, seqUrl);
        _infoLogger = SetupLogger(LogEventLevel.Information, logPath, seqUrl);
        _fatalLogger = SetupLogger(LogEventLevel.Fatal, logPath, seqUrl);
    }

    public void LogError(string message, params object?[]? parameters)
        => _errorLogger.Error(message, parameters);

    public void LogFatal(string message, params object?[]? parameters)
        => _fatalLogger.Fatal(message, parameters);

    public void LogInformation(string message, params object?[]? parameters)
        => _infoLogger.Information(message, parameters);

    private ILogger SetupLogger(LogEventLevel logEventLevel, string logPath, string seqUrl)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .WriteTo.Seq(seqUrl)
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
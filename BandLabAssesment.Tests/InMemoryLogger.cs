using Microsoft.Extensions.Logging;

namespace BandLabAssesment.Tests;

public class InMemoryLogger : ILogger
{
    private readonly List<string> logMessages;

    public InMemoryLogger()
    {
        logMessages = new List<string>();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        var logEntry = $"LogLevel: {logLevel}, EventId: {eventId}, Message: {message}";
        logMessages.Add(logEntry);
    }

    public List<string> GetLogMessages()
    {
        return logMessages;
    }
}
using Microsoft.Extensions.Logging;

namespace Find_RegisterTest;

/// <summary>
/// A simple ILogger for unit testing
/// </summary>
public class MockLogger : ILogger
{
    public List<string?> LoggedMessages = new List<string?>();

    public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
    public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var msg = formatter.Invoke(state, exception);
        LoggedMessages.Add(msg);
    }
}
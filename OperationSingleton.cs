namespace TestDI;

public class OperationSingleton : Operation, IOperationSingleton
{
    private readonly List<LogInfo> _logEntries = new List<LogInfo>();

    public void AddLogEntry(LogInfo logInfo)
    {
        _logEntries.Add(logInfo);
    }

    public IEnumerable<LogInfo> GetLogEntries()
    {
        return _logEntries;
    }

}
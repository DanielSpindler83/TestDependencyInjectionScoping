namespace TestDI;

public interface IOperation
{
    string OperationId { get; }
}

public interface IOperationTransient : IOperation { }
public interface IOperationScoped : IOperation { }
public interface IOperationSingleton : IOperation 
{
    void AddLogEntry(LogInfo logInfo);

    IEnumerable<LogInfo> GetLogEntries();
}
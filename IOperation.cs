namespace TestDI;

public interface IOperation
{
    string OperationId { get; }
}

// these really only exist so DI service container can differentiate instances and pass back the correctly scoped one
public interface IOperationTransient : IOperation { }
public interface IOperationScoped : IOperation { }

// We extend Singleton so we can store and retrieve each http request log info for the lifetime of the app.
public interface IOperationSingleton : IOperation 
{
    void AddLogEntry(LogInfo logInfo);

    IEnumerable<LogInfo> GetLogEntries();
}
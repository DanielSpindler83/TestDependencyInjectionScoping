namespace TestDI;

public class Operation : IOperationTransient, IOperationScoped
{
    public Operation()
    {
        OperationId = Guid.NewGuid().ToString()[^4..];
    }

    public string OperationId { get; }
}
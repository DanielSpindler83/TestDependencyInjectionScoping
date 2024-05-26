namespace TestDI;

public class LogInfo
{
    public DateTime Timestamp { get; set; }
    public required string TraceIdentifier { get; set; }
    public required string TransientOperationId { get; set; }
    public required string ScopedOperationId { get; set; }
    public required string SingletonOperationId { get; set; }
}

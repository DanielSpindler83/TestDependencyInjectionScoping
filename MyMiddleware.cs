using Microsoft.Extensions.Logging;

namespace TestDI;

public class MyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    private readonly IOperationSingleton _singletonOperation;

    public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger,
        IOperationSingleton singletonOperation)
    {
        _logger = logger;
        _singletonOperation = singletonOperation;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,
        IOperationTransient transientOperation, IOperationScoped scopedOperation)
    {
        _logger.LogInformation("START HTTP REQUEST");
        _logger.LogInformation("MW: HttpContext TraceIdentifier: " + context.TraceIdentifier);
        _logger.LogInformation("MW: Transient: " + transientOperation.OperationId);
        _logger.LogInformation("MW: Scoped: " + scopedOperation.OperationId);
        _logger.LogInformation("MW: Singleton: " + _singletonOperation.OperationId);

        var logInfo = new LogInfo
        {
            Timestamp = DateTime.UtcNow,
            TraceIdentifier = context.TraceIdentifier,
            TransientOperationId = transientOperation.OperationId,
            ScopedOperationId = scopedOperation.OperationId,
            SingletonOperationId = _singletonOperation.OperationId
        };

        _singletonOperation.AddLogEntry(logInfo);

        await _next(context);
        _logger.LogInformation("END OF HTTP REQUEST");
    }
}

public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyMiddleware>();
    }
}
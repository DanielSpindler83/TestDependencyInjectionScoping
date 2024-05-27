using Microsoft.Extensions.Logging;

namespace TestDI;

/*
    We register our own custom middleware component.
    This runs on the http request as it comes in, before it hits our actual app code.
    We register this in program.cs via line =             app.UseMyMiddleware();
    The middleware injects the singleton into its constructor. Persists for lifetime of the app.
    The scoped and transient objects are injected into the main middleware InvokeAsync method.
    The scoped instance will live on for the life of the HTTP request - we use logging to see this (match trace id to different object access to confirm).
    The transient instance will live ONLY for this method call and be disposed of when the below method completes.
    Any further calls for a transient scoped object will result in a new one being created.
    
    NOTE we log into to the console, but also persiste the info into a member of the Singleton object.
    Via a razor page we will show all log entries.
*/

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
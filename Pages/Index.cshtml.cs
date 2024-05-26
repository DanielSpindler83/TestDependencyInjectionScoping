using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestDI.Pages;

public class IndexModel(ILogger<IndexModel> logger,
                  IOperationTransient transientOperation,
                  IOperationScoped scopedOperation,
                  IOperationSingleton singletonOperation,
                  IHttpContextAccessor httpContextAccessor) : PageModel
{
    private readonly ILogger _logger = logger;
    private readonly IOperationTransient _transientOperation = transientOperation;
    private readonly IOperationSingleton _singletonOperation = singletonOperation;
    private readonly IOperationScoped _scopedOperation = scopedOperation;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public IEnumerable<LogInfo> LogEntries { get; private set; } = Enumerable.Empty<LogInfo>();

    public void OnGet()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;


        _logger.LogInformation("Transient: " + _transientOperation.OperationId);
        _logger.LogInformation("Scoped: " + _scopedOperation.OperationId);
        _logger.LogInformation("Singleton: " + _singletonOperation.OperationId);

        var logInfo = new LogInfo
        {
            Timestamp = DateTime.UtcNow,
            TraceIdentifier = httpContext!.TraceIdentifier,
            TransientOperationId = _transientOperation.OperationId,
            ScopedOperationId = _scopedOperation.OperationId,
            SingletonOperationId = _singletonOperation.OperationId
        };

        _singletonOperation.AddLogEntry(logInfo);

        LogEntries = _singletonOperation.GetLogEntries();
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestDI.Pages;

// Super simple razor page get request for index page
// Runs after our middleware - so we can inject all our services again and check their state
// We can see here that the scoped instance from middleware will be same object here(same guid)
// Singleton is same guid for every http request
// Transient is a new instance every single call made
// Each HTTP request has a trace identifier to group things in the output

// NOTE we are using new(C# 12) primary constructor here
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
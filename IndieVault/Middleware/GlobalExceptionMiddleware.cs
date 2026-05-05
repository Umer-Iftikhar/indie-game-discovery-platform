namespace IndieVault.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error");

                var logPath = Path.Combine(_env.ContentRootPath, "errors.log");

                var logMessage = $"[{DateTime.UtcNow}] {ex.Message}, Action: {context.Request.Path}, Location: {ex.StackTrace}, Method: {context.Request.Method}{Environment.NewLine}";
                await File.AppendAllTextAsync(logPath, logMessage);

                if (context.Response.HasStarted)
                {
                    return;
                }
                context.Response.StatusCode = 500;

                context.Response.Redirect("/Error");
            }
        }
    }
}

namespace UserManagementAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request details
            var method = context.Request.Method;
            var path = context.Request.Path;

            // Call the next middleware in the pipeline
            await _next(context);

            // Log response details
            var statusCode = context.Response.StatusCode;
            _logger.LogInformation("HTTP {Method} {Path} responded with {StatusCode}", method, path, statusCode);
        }
    }
}

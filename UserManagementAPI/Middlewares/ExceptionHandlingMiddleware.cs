namespace UserManagementAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details (optional)
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Set response status code and content type
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                // Return a consistent JSON error response
                var errorResponse = new { error = "Internal server error." };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}

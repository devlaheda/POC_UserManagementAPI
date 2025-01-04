namespace UserManagementAPI.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;
        private const string BearerPrefix = "Bearer ";

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Retrieve the Authorization header
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith(BearerPrefix))
            {
                // No Authorization header or invalid format
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var errorResponse = new { error = "Unauthorized. Token is required." };
                await context.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            // Extract the token from the header
            var token = authorizationHeader.Substring(BearerPrefix.Length).Trim();

            // Validate the token (for demonstration purposes, replace with actual validation logic)
            if (!IsValidToken(token))
            {
                // Invalid token
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var errorResponse = new { error = "Unauthorized. Invalid token." };
                await context.Response.WriteAsJsonAsync(errorResponse);
                return;
            }

            // If the token is valid, proceed to the next middleware
            await _next(context);
        }

        // Simulated token validation (replace with actual logic)
        private bool IsValidToken(string token)
        {
            // For demonstration purposes, any non-empty token is considered valid
            // Replace this with real token validation, e.g., JWT validation or checking a token store
            return token == "valid-token";  // Example of a valid token
        }
    }
}

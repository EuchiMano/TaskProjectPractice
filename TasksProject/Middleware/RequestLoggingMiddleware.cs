namespace TasksProject.Middleware
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
            // Call the next middleware in the pipeline
            await _next(context);

            // After the request is processed, check if the user is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userName = context.User.Identity.Name ?? "Unknown";
                var method = context.Request.Method;
                var path = context.Request.Path;
                var statusCode = context.Response.StatusCode;

                _logger.LogInformation("Authenticated request: User={User}, Method={Method}, Path={Path}, StatusCode={StatusCode}",
                    userName, method, path, statusCode);
            }
        }
    }
}
using System.Diagnostics;

namespace DevCodeX_API.Middlewares
{
    /// <summary>
    /// Middleware to log incoming HTTP requests and outgoing responses.
    /// Logs metadata including route, method, response code, and completion time.
    /// </summary>
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
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var requestTime = DateTime.UtcNow;

            // Log incoming request
            _logger.LogInformation(
                "[REQUEST] {Method} {Path} - Started at {Time}",
                requestMethod,
                requestPath,
                requestTime.ToString("yyyy-MM-dd HH:mm:ss.fff")
            );

            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;
                var completionTime = stopwatch.ElapsedMilliseconds;

                // Log response metadata
                _logger.LogInformation(
                    "[RESPONSE] {Method} {Path} - Status: {StatusCode} - Completed in {CompletionTime}ms",
                    requestMethod,
                    requestPath,
                    statusCode,
                    completionTime
                );

                // Log warnings for slow requests (>1000ms)
                if (completionTime > 1000)
                {
                    _logger.LogWarning(
                        "[SLOW REQUEST] {Method} {Path} took {CompletionTime}ms",
                        requestMethod,
                        requestPath,
                        completionTime
                    );
                }

                // Log errors for 4xx and 5xx status codes
                if (statusCode >= 400)
                {
                    var logLevel = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;
                    _logger.Log(
                        logLevel,
                        "[HTTP {StatusCode}] {Method} {Path} - Completed in {CompletionTime}ms",
                        statusCode,
                        requestMethod,
                        requestPath,
                        completionTime
                    );
                }
            }
        }
    }
}

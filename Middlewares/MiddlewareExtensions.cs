namespace DevCodeX_API.Middlewares
{
    /// <summary>
    /// Extension methods for registering custom middlewares.
    /// Follows Open/Closed Principle - easy to extend with new middlewares.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds request logging middleware to the application pipeline.
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}

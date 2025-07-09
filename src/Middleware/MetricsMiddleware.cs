using Prometheus;
using CompanyService.Services;

namespace CompanyService.Middleware
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MetricsMiddleware> _logger;

        public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var method = context.Request.Method;
            var endpoint = context.Request.Path.Value ?? "/";

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode.ToString();

                // Record metrics
                MetricsService.RequestCounter
                    .WithLabels(method, endpoint, statusCode)
                    .Inc();

                MetricsService.RequestDuration
                    .WithLabels(method, endpoint)
                    .Observe(stopwatch.Elapsed.TotalSeconds);

                _logger.LogDebug("Request {Method} {Endpoint} completed with status {StatusCode} in {Duration}ms",
                    method, endpoint, statusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
} 
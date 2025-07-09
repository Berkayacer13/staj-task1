using Microsoft.Extensions.Options;

namespace CompanyService.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeyOptions> options, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _apiKey = options.Value.ApiKey;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for health and metrics endpoints
            if (context.Request.Path.StartsWithSegments("/health") || 
                context.Request.Path.StartsWithSegments("/metrics") ||
                context.Request.Path.StartsWithSegments("/"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKeyHeader))
            {
                _logger.LogWarning("API request without X-API-Key header from {IP}", context.Connection.RemoteIpAddress);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is required");
                return;
            }

            var providedApiKey = apiKeyHeader.FirstOrDefault();
            if (string.IsNullOrEmpty(providedApiKey) || providedApiKey != _apiKey)
            {
                _logger.LogWarning("Invalid API Key provided from {IP}", context.Connection.RemoteIpAddress);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            _logger.LogInformation("Valid API Key used for request to {Path} from {IP}", 
                context.Request.Path, context.Connection.RemoteIpAddress);
            
            await _next(context);
        }
    }

    public class ApiKeyOptions
    {
        public string ApiKey { get; set; } = string.Empty;
    }
} 
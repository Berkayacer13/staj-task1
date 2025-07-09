using Prometheus;

namespace CompanyService.Services
{
    public class MetricsService
    {
        // HTTP request metrics
        public static readonly Counter RequestCounter = Metrics.CreateCounter(
            "http_requests_total", 
            "Total number of HTTP requests",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint", "status_code" }
            });

        public static readonly Histogram RequestDuration = Metrics.CreateHistogram(
            "http_request_duration_seconds",
            "HTTP request duration in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "method", "endpoint" },
                Buckets = new[] { 0.1, 0.25, 0.5, 1, 2.5, 5, 10 }
            });

        // Cache metrics
        public static readonly Counter CacheHitCounter = Metrics.CreateCounter(
            "cache_hits_total",
            "Total number of cache hits",
            new CounterConfiguration
            {
                LabelNames = new[] { "cache_key" }
            });

        public static readonly Counter CacheMissCounter = Metrics.CreateCounter(
            "cache_misses_total",
            "Total number of cache misses",
            new CounterConfiguration
            {
                LabelNames = new[] { "cache_key" }
            });

        // Database metrics
        public static readonly Histogram DatabaseQueryDuration = Metrics.CreateHistogram(
            "database_query_duration_seconds",
            "Database query duration in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "query_type" },
                Buckets = new[] { 0.01, 0.05, 0.1, 0.25, 0.5, 1, 2.5, 5 }
            });

        // Business metrics
        public static readonly Gauge ActiveConnections = Metrics.CreateGauge(
            "active_connections",
            "Number of active connections");

        public static readonly Counter TotalCompanies = Metrics.CreateCounter(
            "total_companies_processed",
            "Total number of companies processed",
            new CounterConfiguration
            {
                LabelNames = new[] { "operation" }
            });
    }
} 
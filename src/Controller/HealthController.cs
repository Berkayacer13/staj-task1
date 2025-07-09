using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using CompanyService.Data;

namespace CompanyService.Controller
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<HealthController> _logger;

        public HealthController(AppDbContext context, IDistributedCache cache, ILogger<HealthController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Health status of the application</returns>
        [HttpGet]
        public async Task<IActionResult> GetHealth()
        {
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Services = new
                {
                    Application = "Healthy",
                    Database = await CheckDatabaseHealth(),
                    Redis = await CheckRedisHealth()
                }
            };

            var overallStatus = healthStatus.Services.Database == "Healthy" && 
                               healthStatus.Services.Redis == "Healthy" ? "Healthy" : "Unhealthy";

            healthStatus = new
            {
                Status = overallStatus,
                Timestamp = DateTime.UtcNow,
                Services = healthStatus.Services
            };

            _logger.LogInformation("Health check performed - Status: {Status}", overallStatus);

            return overallStatus == "Healthy" ? Ok(healthStatus) : StatusCode(503, healthStatus);
        }

        private async Task<string> CheckDatabaseHealth()
        {
            try
            {
                await _context.Database.CanConnectAsync();
                return "Healthy";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return "Unhealthy";
            }
        }

        private async Task<string> CheckRedisHealth()
        {
            try
            {
                await _cache.GetStringAsync("health-check");
                return "Healthy";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis health check failed");
                return "Unhealthy";
            }
        }
    }
} 
using Microsoft.AspNetCore.Mvc;
using CompanyService.DTO;
using CompanyService.Services;

namespace CompanyService.Controller
{
    [ApiController]
    [Route("api/external/companies")]
    public class ExternalCompaniesController : ControllerBase
    {
        private readonly IExternalCompanyService _externalCompanyService;
        private readonly ILogger<ExternalCompaniesController> _logger;

        public ExternalCompaniesController(IExternalCompanyService externalCompanyService, ILogger<ExternalCompaniesController> logger)
        {
            _externalCompanyService = externalCompanyService;
            _logger = logger;
        }

        /// <summary>
        /// Get a random company from external API
        /// </summary>
        /// <returns>Random company data</returns>
        [HttpGet("random")]
        public async Task<ActionResult<ExternalCompanyDto>> GetRandomCompany()
        {
            try
            {
                var company = await _externalCompanyService.GetRandomCompanyAsync();
                return Ok(company);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to get random company from external API");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting random company");
                return StatusCode(500, new { error = "Internal server error occurred" });
            }
        }

        /// <summary>
        /// Get multiple random companies from external API
        /// </summary>
        /// <param name="count">Number of companies to fetch (1-100)</param>
        /// <returns>List of random companies</returns>
        [HttpGet("random/{count}")]
        public async Task<ActionResult<List<ExternalCompanyDto>>> GetMultipleRandomCompanies(int count)
        {
            try
            {
                var companies = await _externalCompanyService.GetMultipleRandomCompaniesAsync(count);
                return Ok(companies);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid count parameter provided");
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to get multiple companies from external API");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting multiple companies");
                return StatusCode(500, new { error = "Internal server error occurred" });
            }
        }
    }
} 
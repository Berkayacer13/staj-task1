using System.Net.Http;
using System.Text.Json;
using CompanyService.DTO;

namespace CompanyService.Services
{
    public class ExternalCompanyService : IExternalCompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalCompanyService> _logger;

        public ExternalCompanyService(IHttpClientFactory httpClientFactory, ILogger<ExternalCompanyService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<ExternalCompanyDto> GetRandomCompanyAsync()
        {
            try
            {
                var requestUri = "https://randomuser.me/api/?inc=login,name,location&noinfo";
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var apiResult = JsonSerializer.Deserialize<RandomUserResponse>(json);
                
                if (apiResult?.Results == null || apiResult.Results.Length == 0)
                {
                    throw new InvalidOperationException("No results returned from external API");
                }

                var user = apiResult.Results[0];
                return MapToCompanyDto(user);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed when calling external API");
                throw new InvalidOperationException($"External API request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization failed for external API response");
                throw new InvalidOperationException($"Failed to parse external API response: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching random company");
                throw;
            }
        }

        public async Task<List<ExternalCompanyDto>> GetMultipleRandomCompaniesAsync(int count)
        {
            if (count <= 0 || count > 100)
            {
                throw new ArgumentException("Count must be between 1 and 100", nameof(count));
            }

            try
            {
                var requestUri = $"https://randomuser.me/api/?inc=login,name,location&noinfo&results={count}";
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var apiResult = JsonSerializer.Deserialize<RandomUserResponse>(json);
                
                if (apiResult?.Results == null)
                {
                    throw new InvalidOperationException("No results returned from external API");
                }

                return apiResult.Results.Select(MapToCompanyDto).ToList();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed when calling external API for multiple companies");
                throw new InvalidOperationException($"External API request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization failed for external API response");
                throw new InvalidOperationException($"Failed to parse external API response: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching multiple random companies");
                throw;
            }
        }

        private static ExternalCompanyDto MapToCompanyDto(RandomUser user)
        {
            return new ExternalCompanyDto
            {
                Id = user.Login.Uuid,
                Name = $"{user.Name.First} {user.Name.Last} Company",
                Address = $"{user.Location.Street.Number} {user.Location.Street.Name}, {user.Location.City}, {user.Location.State}, {user.Location.Country}",
                Domain = $"{user.Login.Username}.com"
            };
        }
    }
} 
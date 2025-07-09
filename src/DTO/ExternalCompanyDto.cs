using System.Text.Json.Serialization;

namespace CompanyService.DTO
{
    // DTOs to map API response
    public class RandomUserResponse
    {
        [JsonPropertyName("results")]
        public RandomUser[] Results { get; set; } = Array.Empty<RandomUser>();
    }

    public class RandomUser
    {
        [JsonPropertyName("login")]
        public LoginInfo Login { get; set; } = new();
        
        [JsonPropertyName("name")]
        public NameInfo Name { get; set; } = new();
        
        [JsonPropertyName("location")]
        public LocationInfo Location { get; set; } = new();
    }

    public class LoginInfo
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; } = string.Empty;
        
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
    }

    public class NameInfo
    {
        [JsonPropertyName("first")]
        public string First { get; set; } = string.Empty;
        
        [JsonPropertyName("last")]
        public string Last { get; set; } = string.Empty;
    }

    public class LocationInfo
    {
        [JsonPropertyName("street")]
        public StreetInfo Street { get; set; } = new();
        
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;
        
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;
        
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class StreetInfo
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    // Our mapped company DTO
    public class ExternalCompanyDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }
} 
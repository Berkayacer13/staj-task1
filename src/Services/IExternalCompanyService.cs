using CompanyService.DTO;

namespace CompanyService.Services
{
    public interface IExternalCompanyService
    {
        Task<ExternalCompanyDto> GetRandomCompanyAsync();
        Task<List<ExternalCompanyDto>> GetMultipleRandomCompaniesAsync(int count);
    }
} 
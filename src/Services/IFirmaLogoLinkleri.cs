using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyService.DTO;

namespace CompanyService.Services
{
    public interface IFirmaLogoLinkleriService
    {
        Task<List<LogoLinkDto>> GetLinksByHisseAdiAsync(string HisseAdi);
        Task<List<LogoLinkDto>> GetLinksByFirmaIdAsync(int firmaId);
        Task<LogoLinkDto> CreateLogoLinkAsync(LogoLinkDto logoLinkDto);
        Task<LogoLinkDto> UpdateLogoLinkAsync(int firmaId, string hisseAdi, LogoLinkDto logoLinkDto);
        Task<bool> DeleteLogoLinkAsync(int firmaId, string hisseAdi);
    }
}
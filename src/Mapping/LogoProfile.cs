using AutoMapper;
using CompanyService.Model;
using CompanyService.DTO;

namespace CompanyService.Mapping
{
    public class LogoProfile : Profile
    {
        public LogoProfile()
        {
            CreateMap<FirmaLogoLinkleri, LogoLinkDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.LogoLink))
                .ForMember(dest => dest.FirmaId, opt => opt.MapFrom(src => src.Firma_Id))
                .ForMember(dest => dest.HisseAdi, opt => opt.MapFrom(src => src.Hisse_Adi));
        }
    }
}
    
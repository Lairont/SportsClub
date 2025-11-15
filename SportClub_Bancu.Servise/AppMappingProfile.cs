using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;

namespace SportClub_Bancu.Servise
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDb>().ReverseMap(); 
            CreateMap<User, LoginViewModel>().ReverseMap(); 
            CreateMap<User, RegisterViewModel>().ReverseMap();
        }
    }
}

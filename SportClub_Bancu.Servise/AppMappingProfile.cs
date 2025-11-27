using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.ViewModels;
using SportClub_Bancu.Domain.ViewModels.LoginAndRegistration;
using SportsClub_Bancu.Domain.ViewModels;

namespace SportClub_Bancu.Servise
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDb>().ReverseMap(); 
            CreateMap<User, LoginViewModel>().ReverseMap(); 
            CreateMap<User, RegisterViewModel>().ReverseMap();
            CreateMap<Inventory, InventoryViewModel>().ReverseMap();
            CreateMap<Inventory, InventoryDb>().ReverseMap();
            CreateMap<RegisterViewModel, ConfirmEmailViewModel>().ReverseMap();
            CreateMap<User, ConfirmEmailViewModel>().ReverseMap();

        }
    }
}

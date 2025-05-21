using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Entity;
using AutoMapper;
using finance_control.Application.ExpenseCQ.ViewModels;

namespace finance_control.Application.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.RefreshToken, x => x.AllowNull())
                .ForMember(dest => dest.PasswordHash, x => x.AllowNull())
                .ForMember(dest => dest.RefreshTokenExpirationTime, map => map.MapFrom(src => AddTenDays()))
                .ForMember(dest => dest.PasswordHash, map => map.MapFrom(src => src.Password));
  
            CreateMap<User, RefreshTokenViewModel>()
                .ForMember(x => x.TokenJwt, x => x.AllowNull());

            CreateMap<RefreshTokenViewModel, UserInfoViewModel>();

            CreateMap<User, UserViewModel>()
               .ForMember(dest => dest.RoleName, map => map.MapFrom(src => src.Role.Name));

            CreateMap<Expenses, ExpenseViewModel>().ReverseMap();

        }

        private static DateTime AddTenDays() { return DateTime.Now.AddDays(10); }
    }
}

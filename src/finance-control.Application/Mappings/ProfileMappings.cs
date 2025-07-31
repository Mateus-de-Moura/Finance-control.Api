using System.Globalization;
using AutoMapper;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.RevenuesCQ.ViewModels;
using finance_control.Application.TransactionsCQ.Command;
using finance_control.Application.TransactionsCQ.ViewModels;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Entity;
using finance_control.Application.CategoryCQ.ViewModels;
using finance_control.Domain.Enum;

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


            CreateMap<Expenses, ExpenseViewModel>()
                .ForMember(dest => dest.CategoryName, map => map.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.DueDate, map => map.MapFrom(src => src.DueDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Value, map => map.MapFrom(src => src.Value.ToString("C", new CultureInfo("pt-BR"))));


            CreateMap<Revenues, RevenuesViewModel>()
                .ForMember(dest => dest.Category, map => map.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Date, map => map.MapFrom(src => src.Date.HasValue ? src.Date.Value.ToString("dd/MM/yyyy") : string.Empty))
                .ForMember(dest => dest.Value,map => map.MapFrom(src => src.Value.ToString("C", new CultureInfo("pt-BR"))));
           
            CreateMap<Category, CategoryViewModel>();

            CreateMap<Transactions, TransactionsViewModel>()
                .ForMember(dest => dest.Type, map => map.MapFrom(src => TypesEnum.FromValue(src.Type).Name))
                .ForMember(dest => dest.PaymentMethod, map => map.MapFrom(src => PaymentMethodEnum.FromValue(src.PaymentMethod).Name))
                .ForMember(dest => dest.Status, map => map.MapFrom(src => StatusPaymentEnum.FromValue(src.Status).Name))
                .ForMember(dest => dest.Category, map => map.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.TransactionType, map => map.MapFrom(src => src.Category.Type))
                .ForMember(dest => dest.Value, map => map.MapFrom(src => src.Value.ToString("C", new CultureInfo("pt-BR"))))
                .ForMember(dest => dest.TransactionDate, map => map.MapFrom(src => src.TransactionDate.ToString("dd/MM/yyyy")));
    
            CreateMap<UpdateTransactionCommand, Transactions>();

            CreateMap<UpdateUserCommand, User>()
                .ForMember(dest => dest.AppRoleId, map => map.MapFrom(src => src.RoleId));
        }

        private static DateTime AddTenDays() { return DateTime.Now.AddDays(10); }
    }
}

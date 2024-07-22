using AutoMapper;
using Task.Domain.Entities;
using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.Models.Users;

namespace TaskAdmin.Web.MapperConfigurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterModel, User>();
        CreateMap<User, UserViewModel>();
        CreateMap<RegisterModel, User>();
        CreateMap<User, LoginViewModel>();
    }
}

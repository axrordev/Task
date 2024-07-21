using AutoMapper;
using Task.Domain.Entities;
using Task.WebApi.Models.Users;

namespace Task.WebApi.MapperConfigurations;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<UserRegisterModel, User>();
        CreateMap<User, UserViewModel>();
        CreateMap<UserRegisterModel, User>();
        CreateMap<User, LoginViewModel>();
    }
}

using AutoMapper;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using UserService.Domain.Entities;

namespace UserService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, CreateUserDto>();
    }
}

using AutoMapper;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using UserService.Application.Mappers;
using UserService.Domain.Entities;

namespace UserService.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom<UserRoleToListResolver>());

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom<ListToUserRoleResolver>());

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom<ListToUserRoleResolver>());
    }
}

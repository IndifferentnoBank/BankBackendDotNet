using AutoMapper;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Mappers
{
    public class UserRoleToListResolver : IValueResolver<User, UserDto, List<UserRole>>
    {
        public List<UserRole> Resolve(User source, UserDto destination, List<UserRole> destMember, ResolutionContext context)
        {
            return Enum.GetValues(typeof(UserRole))
                       .Cast<UserRole>()
                       .Where(r => r != 0 && source.Role.HasFlag(r))
                       .ToList();
        }
    }

    public class ListToUserRoleResolver :
        IValueResolver<UserDto, User, UserRole>,
        IValueResolver<CreateUserDto, User, UserRole>
    {
        public UserRole Resolve(UserDto source, User destination, UserRole destMember, ResolutionContext context)
        {
            return source.Role.Aggregate(UserRole.CUSTOMER, (current, role) => current | role);
        }

        public UserRole Resolve(CreateUserDto source, User destination, UserRole destMember, ResolutionContext context)
        {
            return source.Role.Aggregate(UserRole.CUSTOMER, (current, role) => current | role);
        }
    }
}

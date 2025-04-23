using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(CreateUserDto createUserDto);
        Task<UserDto> UpdateUser(Guid userId, Guid id, CreateUserDto createUserDto);
        Task LockUnlockUser(Guid userId, Guid id, bool isLocked);
        Task<UserDto> GetUserById(Guid userId, Guid id);
        Task<ShortenUserDto> GetUserByPhone(string phone);
        //Task<String> LoginUser(LoginUserDto loginUserDto);
        Task<List<UserDto>> GetAllUsers(Guid userId);
        
    }
}

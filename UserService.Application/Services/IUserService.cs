using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(CreateUserDto createUserDto);
        Task<UserDto> UpdateUser(Guid id, CreateUserDto createUserDto);
        Task LockUnlockUser(Guid userId, bool isLocked);
        Task<UserDto> GetUserById(Guid userId);
        Task<UserDto> GetUserByPhone(string phone);
        Task<List<UserDto>> GetAllUsers();
    }
}

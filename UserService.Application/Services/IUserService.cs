using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using UserService.Domain.Entities;

namespace UserService.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(CreateUserDto createUserDto);
        Task<UserDto> UpdateUser(Guid userId, Guid id, CreateUserDto createUserDto);
        Task LockUnlockUser(Guid userId, Guid id, bool isLocked);
        Task<UserDto> GetUserById(Guid userId, Guid id);
        Task<String> GetUserByPhone(string phone);
        //Task<String> LoginUser(LoginUserDto loginUserDto);
        Task<List<UserDto>> GetAllUsers(Guid userId);
        
    }
}

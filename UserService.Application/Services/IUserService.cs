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
        Task<UserDto> UpdateUser(Guid id, CreateUserDto createUserDto);
        Task LockUnlockUser(Guid id, bool isLocked);
        Task<UserDto> GetUserById(Guid id);
        Task<String> GetUserByPhone(string phone);
        //Task<String> LoginUser(LoginUserDto loginUserDto);
        Task<List<UserDto>> GetAllUsers();
        
    }
}

using Common.Exceptions;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using AutoMapper;
using UserSevice.Persistence.Repositories.UserRepository;
using UserService.Domain.Entities;
using System.Data;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
        {
            if (await _userRepository.CheckIfUserExistsByPhone(createUserDto.PhoneNumber))
                throw new BadRequest($"User with {createUserDto.PhoneNumber} already exist");

            if (await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with {createUserDto.Email} already exist");

            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(createUserDto.Password, 11);

            var user = await _userRepository.CreateUserAsync(createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, createUserDto.Role, passwordHash);
            if (user == null)
                throw new BadRequest("User could not be created.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<String> LoginUser(LoginUserDto loginUserDto)
        {
            if (!await _userRepository.CheckIfUserExistsByPhone(loginUserDto.PhoneNumber))
                throw new NotFound($"User with {loginUserDto.PhoneNumber} not found");

            var user = await _userRepository.GetUserByPhoneAsync(loginUserDto.PhoneNumber);

            if (!BCrypt.Net.BCrypt.EnhancedVerify(loginUserDto.Password, user.Password))
                throw new BadRequest("Wrong password");
            return user.Id.ToString();
        }

        public async Task<UserDto> UpdateUser(Guid id, CreateUserDto createUserDto)
        {
            if (!await _userRepository.CheckIfUserExistsById(id))
                throw new NotFound($"User with {id} not found");

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user.PhoneNumber != createUserDto.PhoneNumber && await _userRepository.CheckIfUserExistsByPhone(createUserDto.PhoneNumber))
                throw new BadRequest($"User with {createUserDto.PhoneNumber} already exist");

            if (user.Email != createUserDto.Email && await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with {createUserDto.Email} already exist");

            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(createUserDto.Password, 11);

            user = await _userRepository.UpdateUserAsync(id, createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, createUserDto.Role, passwordHash);

            return _mapper.Map<UserDto>(user);
        }

        public async Task LockUnlockUser(Guid id, bool isLocked)
        {
            var success = await _userRepository.LockUnlockUserAsync(id, isLocked);
            if (!success)
                throw new NotFound("User not found.");
        }

        public async Task<UserDto> GetUserById(Guid id)
        {

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new NotFound("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<String> GetUserByPhone(string phone)
        {
            var user = await _userRepository.GetUserByPhoneAsync(phone);
            if (user == null)
                throw new NotFound("User not found.");

            return user.Id.ToString();
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
          
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}

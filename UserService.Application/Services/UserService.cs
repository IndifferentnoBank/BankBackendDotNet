using Common.Exceptions;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using AutoMapper;
using UserSevice.Persistence.Repositories.UserRepository;
using UserService.Domain.Entities;

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
            if (!await _userRepository.CheckIfUserExistsByPhone(createUserDto.PhoneNumber))
                throw new BadRequest($"User with {createUserDto.PhoneNumber} already exist");

            if (!await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with {createUserDto.Email} already exist");

            var user = await _userRepository.CreateUserAsync(createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, createUserDto.Role);
            if (user == null)
                throw new BadRequest("User could not be created.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUser(Guid id, CreateUserDto createUserDto)
        {
            if (!await _userRepository.CheckIfUserExistsById(id))
                throw new NotFound($"User with {id} not found");

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user.PhoneNumber != createUserDto.PhoneNumber && !await _userRepository.CheckIfUserExistsByPhone(createUserDto.PhoneNumber))
                throw new BadRequest($"User with {createUserDto.PhoneNumber} already exist");

            if (user.Email != createUserDto.Email && !await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with {createUserDto.Email} already exist");

            user = await _userRepository.UpdateUserAsync(id, createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, createUserDto.Role);

            return _mapper.Map<UserDto>(user);
        }

        public async Task LockUnlockUser(Guid userId, bool isLocked)
        {
            var success = await _userRepository.LockUnlockUserAsync(userId, isLocked);
            if (!success)
                throw new NotFound("User not found.");
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFound("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByPhone(string phone)
        {
            var user = await _userRepository.GetUserByPhoneAsync(phone);
            if (user == null)
                throw new NotFound("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}

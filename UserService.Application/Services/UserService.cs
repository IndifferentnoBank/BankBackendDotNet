using Common.Exceptions;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;
using AutoMapper;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Persistence.Repositories.UserRepository;

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
                throw new BadRequest($"User with phone number {createUserDto.PhoneNumber} already exist");

            if (await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with email {createUserDto.Email} already exist");

            User newUser = _mapper.Map<User>(createUserDto);

            var user = await _userRepository.CreateUserAsync(createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, newUser.Role);
            if (user == null)
                throw new BadRequest("User could not be created.");

            return _mapper.Map<UserDto>(user);
        }
        

        public async Task<UserDto> UpdateUser(Guid userId, Guid id, CreateUserDto createUserDto)
        {
            var userCheck = await _userRepository.GetUserByIdAsync(userId);

            if (userId != id && userCheck.Role == UserRole.CUSTOMER)
                throw new Forbidden("User does not have rights");

            if (!await _userRepository.CheckIfUserExistsById(id))
                throw new NotFound($"User with {id} not found");

            var user = await _userRepository.GetUserByIdAsync(id);

            if (user.PhoneNumber != createUserDto.PhoneNumber && await _userRepository.CheckIfUserExistsByPhone(createUserDto.PhoneNumber))
                throw new BadRequest($"User with phone number {createUserDto.PhoneNumber} already exist");

            if (user.Email != createUserDto.Email && await _userRepository.CheckIfUserExistsByEmail(createUserDto.Email))
                throw new BadRequest($"User with email {createUserDto.Email} already exist");

            User newUser = _mapper.Map<User>(createUserDto);
            user = await _userRepository.UpdateUserAsync(id, createUserDto.FullName, createUserDto.Email, createUserDto.PhoneNumber, createUserDto.Passport, newUser.Role);

            return _mapper.Map<UserDto>(user);
        }

        public async Task LockUnlockUser(Guid userId, Guid id, bool isLocked)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user.Role == UserRole.CUSTOMER)
                throw new Forbidden("User does not have rights");

            var success = await _userRepository.LockUnlockUserAsync(id, isLocked);
            if (!success)
                throw new NotFound("User not found.");
        }

        public async Task<UserDto> GetUserById(Guid userId, Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (userId != id && user.Role == UserRole.CUSTOMER)
                throw new Forbidden("User does not have rights");

            var userById = await _userRepository.GetUserByIdAsync(id);
            if (userById == null)
                throw new NotFound("User not found.");

            return _mapper.Map<UserDto>(userById);
        }

        public async Task<ShortenUserDto> GetUserByPhone(string phone)
        {
            var user = await _userRepository.GetUserByPhoneAsync(phone);
            if (user == null)
                throw new NotFound("User not found.");

            return new ShortenUserDto(user);
        }

        public async Task<List<UserDto>> GetAllUsers(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user.Role == UserRole.CUSTOMER)
                throw new Forbidden("User does not have rights");

            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

    }
}

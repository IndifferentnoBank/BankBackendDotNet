using Common.GenericRepository;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Persistence.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> CreateUserAsync(string name, string email, string phone, string passport, UserRole role);
        Task<User> UpdateUserAsync(Guid userId, string name, string email, string phone, string passport, UserRole role);
        Task<bool> LockUnlockUserAsync(Guid userId, bool isLocked);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByPhoneAsync(string phone);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> CheckIfUserExistsById(Guid userId);
        Task<bool> CheckIfUserExistsByPhone(string phone);
        Task<bool> CheckIfUserExistsByEmail(string email);
    }
}
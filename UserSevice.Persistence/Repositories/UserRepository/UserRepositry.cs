using Common.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserSevice.Persistence.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserServiceDbContext _dbContext;

        public UserRepository(DbContext context, UserServiceDbContext dbContext) : base(context)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(string name, string email, string phone, string passport, UserRole role)
        {
            var user = new User
            {
                FullName = name,
                Email = email,
                PhoneNumber = phone,
                Passport = passport,
                Role = role,
                IsLocked = false 
            };

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> LockUnlockUserAsync(Guid userId, bool isLocked)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return false; 
            }

            user.IsLocked = isLocked;
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return (await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId))!;
        }

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            return (await _dbContext.User.FirstOrDefaultAsync(x => x.PhoneNumber == phone))!;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.User.ToListAsync();
        }

        public async Task<bool> CheckIfUserExistsByEmailAsync(string email)
        {
            return await _dbContext.User.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckIfUserExistsByIdAsync(Guid userId)
        {
            return await _dbContext.User.AnyAsync(x => x.Id == userId);
        }
    }
}
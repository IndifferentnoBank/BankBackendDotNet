using Common.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders.Physical;
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

        public UserRepository(UserServiceDbContext dbContext) : base(dbContext)
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
                //Password = password
            };

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        public async Task<User> UpdateUserAsync(Guid userId, string name, string email, string phone, string passport, UserRole role)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
            user.FullName = name;
            user.Passport = passport;
            user.PhoneNumber = phone;
            user.Email = email;
            user.Role = role;
           // user.Password = password;
            _dbContext.User.Update(user);
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

        public async Task<bool> CheckIfUserExistsByPhone(string phone)
        {
            return await _dbContext.User.AnyAsync(x => x.PhoneNumber == phone);
        }

        public async Task<bool> CheckIfUserExistsById(Guid userId)
        {
            return await _dbContext.User.AnyAsync(x => x.Id == userId);
        }

        public async Task<bool> CheckIfUserExistsByEmail(string email)
        {
            return await _dbContext.User.AnyAsync(x => x.Email == email);
        }
    }
}
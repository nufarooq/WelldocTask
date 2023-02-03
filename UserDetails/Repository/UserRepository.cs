using Microsoft.EntityFrameworkCore;
using System;
using UserDetails.DAL;
using UserDetails.Models;
using UserDetails.Repository.Interface;

namespace UserDetails.Repository
{
    public class UserRepository : IUser
    {
        private readonly UserContext dbContext;
        public UserRepository(UserContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserRegistration> GetUser(Guid userid)
        {
            return await dbContext.Users.FirstOrDefaultAsync(e => e.UserId == userid);
        }

        public async Task<UserRegistration> GetUser(string emailid, string password)
        {
            return await dbContext.Users.FirstOrDefaultAsync(e => e.EmailId == emailid && e.Password == password);
        }

        public async Task<UserRegistration> Register(UserRegistration user)
        {
            user.CreatedOn = DateTime.Now;
            var result = await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateUser(UserRegistration user)
        {
            var result = await dbContext.Users
                .FirstOrDefaultAsync(e => e.UserId == user.UserId);

            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.EmailId = user.EmailId;
                result.DOB = user.DOB;
                result.Gender = user.Gender;
                result.Password = user.Password;
                result.MobileNumber = user.MobileNumber;
                result.ModifiedOn = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Guid> DeleteUser(Guid userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return userId;
        }

    }
}

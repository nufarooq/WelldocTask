using UserDetails.Models;

namespace UserDetails.Repository.Interface
{
    public interface IUser
    {
        Task<UserRegistration> GetUser(Guid userid);
        Task<UserRegistration> GetUser(string emailid, string password);
        Task<UserRegistration> Register(UserRegistration user);
        Task UpdateUser(UserRegistration user);
        Task<Guid> DeleteUser(Guid userId);
    }
}

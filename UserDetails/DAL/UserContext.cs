using Microsoft.EntityFrameworkCore;
using UserDetails.Models;

namespace UserDetails.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }
        public DbSet<UserRegistration> Users { get; set; }

    }
}

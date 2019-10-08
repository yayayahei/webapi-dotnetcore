using Microsoft.EntityFrameworkCore;

namespace Hello.Models
{
    public class HelloDbContext : DbContext
    {
        public HelloDbContext(DbContextOptions<HelloDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;

namespace HttpPatch.Api.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace TestAppK8S.DAL
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }
}

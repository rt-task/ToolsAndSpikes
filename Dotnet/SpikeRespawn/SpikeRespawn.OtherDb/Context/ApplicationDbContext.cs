using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpikeRespawn.OtherDb.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new List<User>
                {
                    new () { Id = 1, Name = "Riccardo", Catchphrase = "Oh no!" },
                    new () { Id = 2, Name = "Daniele", Catchphrase = "Standuppino" },
                    new () { Id = 3, Name = "Giovanni", Catchphrase = "Buzzword!" }
                }
            );
        }
    }
}
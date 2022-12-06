using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using SpikeRespawn.SqlServer.Context;

namespace SpikeRespawn.Tests
{
    public static class TestHelpers
    {
        public static async Task ResetDatabase(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Server.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
        }

        public static async Task ResetDatabaseWithRespawn(CustomWebApplicationFactory factory, string connectionString)
        {
            var checkpoint = CustomWebApplicationFactory.Checkpoint;
            await checkpoint.Reset(connectionString);
            
            await SeedDatabase(factory);

        }

        private static async Task SeedDatabase(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var users = new List<User>
            {
                new() {Name = "Test1", Catchphrase = "Test1"},
                new() {Name = "Test2", Catchphrase = "Test2"},
                new() {Name = "Test3", Catchphrase = "Test3"}
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

    }
}

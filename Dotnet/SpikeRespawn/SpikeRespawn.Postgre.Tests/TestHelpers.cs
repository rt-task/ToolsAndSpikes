using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using SpikeRespawn.OtherDb.Context;

namespace SpikeRespawn.Postgre.Tests
{
    public static class TestHelpers
    {
        private static Checkpoint checkpoint = new ()
        {
            SchemasToInclude = new[]
            {
                "public"
            },
            DbAdapter = DbAdapter.Postgres
        };

        public static async Task ResetDatabase(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Server.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
        }

        public static async Task ResetDatabaseWithRespawn(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await checkpoint.Reset(connection);
        }
    }
}
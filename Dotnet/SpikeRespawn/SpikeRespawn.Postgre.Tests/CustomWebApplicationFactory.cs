using Microsoft.AspNetCore.Mvc.Testing;
using Respawn;
using SpikeRespawn.OtherDb;

namespace SpikeRespawn.Postgre.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
    }
}
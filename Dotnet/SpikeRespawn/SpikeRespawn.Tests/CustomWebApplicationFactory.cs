using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Respawn;
using SpikeRespawn.SqlServer;

namespace SpikeRespawn.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public static Checkpoint Checkpoint => new ()
        {
            WithReseed = true
        };
    }
}

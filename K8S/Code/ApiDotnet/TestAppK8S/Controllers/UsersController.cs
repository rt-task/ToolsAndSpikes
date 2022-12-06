using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using TestAppK8S.DAL;

namespace TestAppK8S.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TestDbContext _ctx;
        private User _inmemoryUser;

        public UsersController(TestDbContext ctx)
        {
            _ctx = ctx;
            _ctx.Database.MigrateAsync().GetAwaiter().GetResult();
            _inmemoryUser = new() { Id = 0, Name = "in memory" };
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _ctx.Users.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var user = _ctx.Users.Add(_inmemoryUser);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), user.Entity);
        }

    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpikeRespawn.OtherDb.Context;

namespace SpikeRespawn.OtherDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        public readonly ApplicationDbContext _ctx;

        public UsersController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            _ctx.Users.Attach(user).State = EntityState.Deleted;
            var rowAffected = await _ctx.SaveChangesAsync();

            if (rowAffected < 1)
                return UnprocessableEntity();

            return Ok(user);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
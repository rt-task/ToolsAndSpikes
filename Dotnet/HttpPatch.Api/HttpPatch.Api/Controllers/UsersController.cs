using HttpPatch.Api.DAL;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HttpPatch.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public UsersController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id}")]
        [ActionName("GetUserById")]
        public async Task<IActionResult> Get(int id) =>
            Ok(await _ctx.Users.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction("GetUserById", new { id = user.Id }, user);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<User> jsonPatchDocument)
        {
            var user = await _ctx.Users.FindAsync(id);

            if (user is null)
                return NotFound(id);

            jsonPatchDocument.ApplyTo(user, ModelState);

            // validate user

            await _ctx.SaveChangesAsync();

            if (ModelState.IsValid)
                return Ok(user);

            return BadRequest(ModelState);
        }
    }
}

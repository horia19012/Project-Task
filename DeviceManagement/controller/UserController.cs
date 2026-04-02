using DeviceManagement.model;
using DeviceManagement.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _service.GetUsersAsync();
            return Ok(users);
            
        }

        [HttpGet("{id}")]
        public async Task <ActionResult> GetUser(int id)
        {
            return Ok( await _service.GetUserAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {
            return Ok (await _service.AddUserAsync(user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id , [FromBody] User user)
        {
            if (id != user.Id) return BadRequest();

            var updatedUser = await _service.UpdateUserAsync(user);

            if(updatedUser == null) return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _service.DeleteUserAsync(id);
            if(!user) return NotFound();
            
            return NoContent();
        }


    }
}
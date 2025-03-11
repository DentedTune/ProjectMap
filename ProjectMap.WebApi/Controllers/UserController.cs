using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;

namespace ProjectMap.WebApi.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet(Name = "ReadUsers")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userRepository.ReadAsync();
            foreach (User user in users)
            {
                user.Username = user.Username.Trim();
            }
            return Ok(users);
        }


        [HttpGet("{username}", Name = "ReadUser")]
        public async Task<ActionResult<User>> Get(string username)
        {
            var user = await _userRepository.ReadAsync(username);
            if (user == null)
                return NotFound();

            user.Username = user.Username.Trim();
            return Ok(user);
        }

        [HttpPost("Register", Name = "CreateUser")]
        public async Task<ActionResult> Add(User user)
        {
            var createdUser = await _userRepository.InsertAsync(user);
            return Created();
        }

        [HttpPost("Login", Name ="Login")]
        public async Task<ActionResult> Login(User user)
        {
            User? wantedUser = await _userRepository.ReadAsync(user.Username);
            if(wantedUser.Password.Trim() == user.Password)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{username}", Name = "UpdateUser")]
        public async Task<ActionResult> Update(string username, User newUser)
        {
            var existingUser = await _userRepository.ReadAsync(username);

            if (existingUser == null)
                return NotFound();

            await _userRepository.UpdateAsync(newUser);

            return Ok(newUser);
        }

        [HttpDelete("{username}", Name = "DeleteUserByUsername")]
        public async Task<IActionResult> Update(string username)
        {
            var existingUser = await _userRepository.ReadAsync(username);

            if (existingUser == null)
                return NotFound();

            await _userRepository.DeleteAsync(username);

            return Ok();
        }
    }
}

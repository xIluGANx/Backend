using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CorsWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users = new()
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
            new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
        };

        // GET: api/users
        [HttpGet]
        [EnableCors("AllowAll")] // Apply specific policy to this endpoint
        public IActionResult GetUsers()
        {
            return Ok(_users);
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        [EnableCors("Restricted")] // Apply restricted policy
        public IActionResult GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        [EnableCors("ApiPolicy")] // Apply API policy
        public IActionResult CreateUser(User user)
        {
            if (user == null)
                return BadRequest();

            user.Id = _users.Max(u => u.Id) + 1;
            _users.Add(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/1
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            return Ok(existingUser);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

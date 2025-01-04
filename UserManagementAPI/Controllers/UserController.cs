using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<User> Users = new List<User>();

        static UserController()
        {
            // Initialize with 110 fake users
            for (int i = 1; i <= 110; i++)
            {
                Users.Add(new User
                {
                    Id = i,
                    Name = $"User{i}",
                    Email = $"user{i}@example.com"
                });
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            const int defaultPage = 1;
            const int defaultPageSize = 10;

            int currentPage = page ?? defaultPage;
            int currentPageSize = pageSize ?? defaultPageSize;

            if (currentPage < 1 || currentPageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var paginatedUsers = Users
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Ok(new
            {
                Page = currentPage,
                PageSize = currentPageSize,
                TotalCount = Users.Count,
                Users = paginatedUsers
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = Users.Find(u => u.Id == id);
            if (user is null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (!user.Validate(out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            Users.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = Users.Find(u => u.Id == id);
            if (user is null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            if (!updatedUser.Validate(out var errorMessage))
            {
                return BadRequest(errorMessage);
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.Find(u => u.Id == id);
            if (user is null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            Users.Remove(user);
            return NoContent();
        }
    }
}

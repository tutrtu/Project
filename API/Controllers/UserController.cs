using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QandAContext _context;
        public UserController(QandAContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string email, string pass)
        {
            var data = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == pass);
            if (data == null)
            {
                return BadRequest("User not found");
            }
            if (data.Password != pass)
            {
                return BadRequest("Wrong password");
            }
            return Ok(data);
        }
        [HttpPost]
        public IActionResult Post([FromBody] UserDto user
            )
        {
            if (user.Email == null || !user.Email.ToLower().Contains("@"))
            {
                return BadRequest("Not email");
            }
            if (user.Password == null || user.Password.Count() < 8)
            {
                return BadRequest("Not password");
            }
            if (_context.Users.Any(x => x.Email == user.Email))
            {
                return BadRequest("Email already in use");
            }
            var data = new User();
            data.Email = user.Email;
            data.Password = user.Password;
            data.Name = user.Name;
            data.Mobile = user.Mobile;
            data.IsAdmin = false;
            /*if (user.Role != null)
                data.RoleId = user.RoleId;
            else data.RoleId = 2;*/
            _context.Users.Add(data);
            _context.SaveChanges();
            return Ok("Add success");
        }

    }
}

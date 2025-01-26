using Microsoft.AspNetCore.Mvc;
using WbApp.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Form.Models;
using Microsoft.EntityFrameworkCore;
using WbApp.Dto;
namespace WbApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        public readonly MainDbContext _context;

        public readonly HttpClient _httpClient;
        public AuthenticationController(MainDbContext context, HttpClient client)
        {
            _context = context;
            _httpClient = client;
        }

        //for sign up/register
        [HttpPost("register")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            user.Password =BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" , userId=user.Id});
        }
      

        //for login
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginReq loginReq)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginReq.Email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(loginReq.Password,existingUser.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });


            }
            HttpContext.Session.SetString("User Id", existingUser.Id.ToString());
            HttpContext.Session.SetString("User Name", existingUser.Name.ToString());

            return Ok(new { message = "login success", userId = existingUser.Id, userName= existingUser.Name });
        }
    }
}

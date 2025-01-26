using Form.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WbApp.Data;
using WbApp.Models;

namespace WbApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly MainDbContext _context; 

        public WishListController(MainDbContext context)
        {
            _context = context;
        }
        [HttpPost("add_wishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] WishList wishlist)
        {

            var userIdStr = HttpContext.Session.GetString("User Id");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("User not logged in or invalid UserId.");
            }
            wishlist.UserId = userId;

            // Ensure MovieId is a valid string if it's being passed as a string.
            if (string.IsNullOrEmpty(wishlist.MovieId))
            {
                return BadRequest("MovieId cannot be null or empty.");
            }

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return Ok(new {message="movie added to wishlist"});
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            var userId = int.Parse(HttpContext.Session.GetString("User Id"));
            
            if (userId == 0) return Unauthorized();

            var wishlist = await _context.Wishlists.Where(w => w.UserId == userId).ToListAsync();
            return Ok(wishlist);
        }
    } 
}

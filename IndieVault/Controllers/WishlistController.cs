using IndieVault.Data;
using IndieVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndieVault.Controllers
{
    [Authorize(Roles = "Player")]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public WishlistController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var existingEntry = await _context.Wishlists.AnyAsync(w => w.GameId == id && w.UserId == userId);

            if (existingEntry)
            {
                return Json(new { success = false, message = "This game is already in your wishlist." });
            }
            var wishlistEntry = new Wishlist
            {
                GameId = id,
                UserId = userId
            };

            _context.Wishlists.Add(wishlistEntry);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Game added to your wishlist!" });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var wishlistEntry = await _context.Wishlists.FirstOrDefaultAsync(w => w.GameId == id && w.UserId == userId);

            if (wishlistEntry == null)
            {
                return Json(new { success = false, message = "This game is not in your wishlist." });
            }

            _context.Wishlists.Remove(wishlistEntry);
            await _context.SaveChangesAsync();

            return Json (new { success = true, message = "Game removed from your wishlist." });
        }

        [HttpGet]
        public async Task<IActionResult> ViewWishlist ()
        {
            var userId = _userManager.GetUserId(User)!;
            var wishlistGames = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Game)
                .ToListAsync();

            return View(wishlistGames);
        }
    }
}

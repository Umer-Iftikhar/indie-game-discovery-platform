using IndieVault.Data;
using IndieVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IndieVault.Controllers
{
    public class DownloadController : Controller
    {
        private readonly AppDbContext _context;
        public DownloadController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Player,Admin,GameDev")]
        public async Task<IActionResult> Download(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new DownloadHistory
            {
                DownloadDate = DateTime.UtcNow,
                GameId = game.Id,
                UserId = user
            };

            _context.DownloadHistories.Add(model);
            await _context.SaveChangesAsync();
            return Redirect(game.DownloadLink);
        }
    }
}

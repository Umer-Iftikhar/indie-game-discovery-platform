using IndieVault.Data;
using IndieVault.Models;
using IndieVault.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IndieVault.Controllers
{

    [Authorize(Roles = "Player,Admin,GameDev")]
    public class DownloadController : Controller
    {
        private readonly AppDbContext _context;
        public DownloadController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
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

        [HttpGet]
        public async Task<IActionResult> MyDownloads()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(); 
            }
            var gameList = await _context.DownloadHistories
                .Include(g => g.Game)
                .Where(g => g.UserId == currentUserId)
                .Select(g => new DownloadHistoryViewModel
                {
                    GameId = g.Game.Id,
                    GameName = g.Game.Title,
                    DownloadTime = g.DownloadDate
                }).ToListAsync();
            return View(gameList);
        }
    }
}

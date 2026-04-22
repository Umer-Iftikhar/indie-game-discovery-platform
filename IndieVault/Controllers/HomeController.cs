using IndieVault.Models;
using IndieVault.Services;
using IndieVault.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IndieVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly GameBrowseService _gameBrowseService;

        public HomeController(GameBrowseService gameBrowseService)
        {
           _gameBrowseService = gameBrowseService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var (games, totalCount) = await _gameBrowseService.GetBrowseGamesAsync(pageNumber, pageSize);
            var viewModel = new GameBrowseViewModel
            {
                Games = games,
                CurrentPage = pageNumber,
                TotalCount = totalCount,
                TotalPages = (totalCount + pageSize - 1) / pageSize
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}

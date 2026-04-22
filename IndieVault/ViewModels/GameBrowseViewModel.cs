using IndieVault.DTOs;
namespace IndieVault.ViewModels
{
    public class GameBrowseViewModel
    {
        public List<GameBrowseDto> Games { get; set; } = new List<GameBrowseDto>();
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

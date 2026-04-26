using IndieVault.DTOs;

namespace IndieVault.ViewModels
{
    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; } = string.Empty;
        public MostWishlistedGameDto MostWishlistedGames { get; set; } = new();
        public List<UserByRoleDto> UsersByRole { get; set; } = new();
        public int TotalGames { get; set; }
        public int TotalReviews { get; set; }
    }
}

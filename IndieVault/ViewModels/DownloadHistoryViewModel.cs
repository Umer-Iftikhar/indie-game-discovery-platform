using IndieVault.Models;

namespace IndieVault.ViewModels
{
    public class DownloadHistoryViewModel
    {
        public string GameName { get; set; } = string.Empty;
        public DateTime DownloadTime { get; set; } = DateTime.UtcNow;
        public int GameId { get; set; }
    }
}

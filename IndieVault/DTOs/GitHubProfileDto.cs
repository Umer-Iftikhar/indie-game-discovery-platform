
namespace IndieVault.DTOs
{
    public class GitHubProfileDto
    {
        public string ProfileUrl { get; set; }

        public int PublicRepos { get; set; }

        public int TotalStars { get; set; }

        public List<string> TopLanguages { get; set; }
    }
}

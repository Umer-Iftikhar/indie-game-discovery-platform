using IndieVault.DTOs;

namespace IndieVault.Services
{
    public interface IGitHubService
    {
        Task<GitHubProfileDto?> GetProfileAsync(string username);
    }
}

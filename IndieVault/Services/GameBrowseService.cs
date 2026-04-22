using IndieVault.DTOs;
using MySqlConnector;
using Dapper;

namespace IndieVault.Services
{
    public class GameBrowseService
    {
        private readonly IConfiguration _configuration;
        public GameBrowseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<(List<GameBrowseDto> Games, int TotalCount)> GetBrowseGamesAsync(int pageNumber, int pageSize)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new MySqlConnection(connectionString);

            var sql = @"
                    Select g.Title, g.Price, g.ReleaseDate, gen.Name, COALESCE(AVG(re.Rating), 0) AS AverageRatings, g.Id, g.CoverImagePath,
                    (
	                    select u.UserName
                        from  aspnetusers u
                        where u.Id = g.DeveloperId
                    ) AS Developer
                    from games g 
                    left Join genres gen ON g.GenreId = gen.Id
                    Left Join reviews re  ON g.Id = re.GameId
                    GROUP BY g.Title, g.Price, g.ReleaseDate, gen.Name, g.DeveloperId, g.Id, g.CoverImagePath
                    limit @PageSize offset @Offset";
            var result = await connection.QueryAsync<GameBrowseDto>(sql, new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

            var countSql = "SELECT COUNT(*) FROM games";
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

            return (result.ToList(), totalCount);
        }
    }
}

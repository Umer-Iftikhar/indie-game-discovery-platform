using Bogus;
using IndieGamePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IndieGamePlatform.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);
            var (devIds, playerIds) = await SeedUsersAsync(userManager);
            var genres = await SeedGenres(context);
            var engines = await SeedEngines(context);
            var platforms = await SeedPlatforms(context);
            var tags = await SeedTags(context);
        }
        public static async Task<List<Genre>> SeedGenres(AppDbContext context)
        {
            if (await context.Genres.AnyAsync())
            {
                return await context.Genres.ToListAsync();
            }
            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Strategy" },
                new Genre { Name = "Simulation" },
                new Genre { Name = "Puzzle" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Sports" },
                new Genre { Name = "Racing" }
            };
            await context.Genres.AddRangeAsync(genres);
            await context.SaveChangesAsync();
            return genres;
        }
        public static async Task<List<Engine>> SeedEngines(AppDbContext context)
        { 
            if (await context.Engines.AnyAsync())
            {
                return await context.Engines.ToListAsync();
            }
            var engines = new List<Engine>
            {
                new Engine { Name = "Unity" },
                new Engine { Name = "Unreal Engine" },
                new Engine { Name = "Godot" },
                new Engine { Name = "CryEngine" },
                new Engine { Name = "RPG Maker" }
            };
            await context.Engines.AddRangeAsync(engines);
            await context.SaveChangesAsync();
            return engines;
        }
        public static async Task<List<Platform>> SeedPlatforms(AppDbContext context)
        { 
            if (await context.Platforms.AnyAsync())
            {
                return await context.Platforms.ToListAsync();
            }
            var platforms = new List<Platform>
            {
                new Platform { Name = "Windows" },
                new Platform { Name = "macOS" },
                new Platform { Name = "Linux" },
                new Platform { Name = "PlayStation" },
                new Platform { Name = "Mobile (iOS/Android)" }
            };
            await context.Platforms.AddRangeAsync(platforms);
            await context.SaveChangesAsync();
            return platforms;
        }
        public static async Task<List<Tag>> SeedTags(AppDbContext context)
        {
            if (await context.Tags.AnyAsync())
            {
                return await context.Tags.ToListAsync();
            }
            var tags = new List<Tag>
            {
                new Tag { Name = "Multiplayer" },
                new Tag { Name = "Singleplayer" },
                new Tag { Name = "2D" },
                new Tag { Name = "Open World" },
                new Tag { Name = "Pixel Art" },
                new Tag { Name = "3D" },
                new Tag { Name = "Sandbox" }
            };
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();
            return tags;
        }
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "GameDev", "Player" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task<(List<string> devIds, List<string> playerIds)> SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            const string defaultPassword = "Password123";
            var devIds = new List<string>();
            var playerIds = new List<string>();

            var existingDevs = await userManager.GetUsersInRoleAsync("GameDev");
            var existingPlayers = await userManager.GetUsersInRoleAsync("Player");
            if (existingDevs.Any())
            {
                return (existingDevs.Select(u => u.Id).ToList(), existingPlayers.Select(u => u.Id).ToList());
            }

            var adminEmail = "admin@indiehub.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    GithubUserName = "Umer-Iftikhar",
                    CreatedDate = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Setting rule for generating fake users using Bogus
            var faker = new Faker<ApplicationUser>()
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.GithubUserName, f => f.Internet.UserName())
                .RuleFor(u => u.CreatedDate, f => f.Date.Past(2))
                .RuleFor(u => u.EmailConfirmed, true);

            for (int i = 0; i < 10; i++)
            {
                var devUser = faker.Generate();
                var result = await userManager.CreateAsync(devUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(devUser, "GameDev");
                    devIds.Add(devUser.Id);
                }
            }

            for (int i = 0; i < 20; i++)
            {
                var playerUser = faker.Generate();
                var result = await userManager.CreateAsync(playerUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(playerUser, "Player");
                    playerIds.Add(playerUser.Id);
                }
            }
            return (devIds, playerIds);
        }

    }
}

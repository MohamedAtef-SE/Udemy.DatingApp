using API.Data;
using API.Entities;
using System.Text.Json;

namespace API.Extentions
{
    public static class InitialDataSeeding
    {
        public async static Task SeedAsync(this WebApplication app)
        {
            var scope = app.Services.CreateAsyncScope();
            var provider = scope.ServiceProvider;
            var dbContext = provider.GetRequiredService<DataContext>();

            if (!dbContext.Users.Any())
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "DataSeeds", "users.json");

                if (File.Exists(filePath))
                {
                    string usersAsJSON = File.ReadAllText(filePath);

                    var users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(usersAsJSON);
                    if (users is not null)
                    {
                        dbContext.Set<AppUser>().AddRange(users);
                        await dbContext.SaveChangesAsync();
                    }
                }

            }
        }
    }
}

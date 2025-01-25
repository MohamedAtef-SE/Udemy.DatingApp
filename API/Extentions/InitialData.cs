using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Extentions
{
    public static class InitialData
    {
        public async static Task SeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var provider = scope.ServiceProvider;
            var dbContext = provider.GetRequiredService<DataContext>();

            // Update-Database
            await dbContext.Database.MigrateAsync();

            // Data Seeds
            
            if (await dbContext.Users.AnyAsync()) return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "DataSeeds", "UserSeedData.json");

            if (File.Exists(filePath))
            {
                string usersAsJSON = await File.ReadAllTextAsync(filePath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(usersAsJSON,options);

                if(users is null) return;

                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    user.PasswordSalt = hmac.Key;
                    await dbContext.Users.AddAsync(user);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
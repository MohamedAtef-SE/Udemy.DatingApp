using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
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
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<AppRole>>();

            // Update-Database
            await dbContext.Database.MigrateAsync();

            // Data Seeds
            
            if (await userManager.Users.AnyAsync()) return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "DataSeeds", "UserSeedData.json");

            if (File.Exists(filePath))
            {
                string usersAsJSON = await File.ReadAllTextAsync(filePath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var users = JsonSerializer.Deserialize<IEnumerable<AppUser>>(usersAsJSON,options);

                if(users is null) return;

                var roles = new List<AppRole>()
                {
                    new(){Name = "Member"},
                    new(){Name = "Admin"},
                    new(){Name = "Moderator"}
                };


                foreach(var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    //using var hmac = new HMACSHA512();
                    //user.UserName = user.UserName.ToLower();
                    //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    //user.PasswordSalt = hmac.Key;
                    await userManager.CreateAsync(user,"Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");
                }

                var admin = new AppUser()
                {
                    UserName = "admin",
                    KnownAs = "Admin",
                    Gender = "",
                    City = "",
                    Country = "",
                    Created = DateTime.UtcNow   
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Text.Json;

namespace Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;

            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"Data/seed.json");
            string userData = await File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            if (users == null)
                return;

            var roles = new List<AppRole>
            {
                new AppRole { Name = Constants.AdminRole },
                new AppRole { Name = Constants.ModeratorRole },
                new AppRole { Name = Constants.MemberRole }
            };

            foreach (var role in roles) 
            { 
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.CreatedAt = DateTime.UtcNow;
                await userManager.CreateAsync(user, Constants.DefaultPassword);
                await userManager.AddToRoleAsync(user, Constants.MemberRole);
            }

            var admin = new AppUser { UserName = "admin", CreatedAt = DateTime.UtcNow }; 

            await userManager.CreateAsync(admin, Constants.DefaultPassword);
            await userManager.AddToRolesAsync(admin, new[] { Constants.AdminRole, Constants.ModeratorRole });
        }
    }
}

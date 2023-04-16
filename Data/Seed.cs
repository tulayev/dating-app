using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync())
                return;

            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"Data/seed.json");
            var userData = await File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123"));
                user.PasswordSalt = hmac.Key;

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}

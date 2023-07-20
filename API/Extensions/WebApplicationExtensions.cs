using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                    await db.Database.MigrateAsync();
                    await Seed.SeedUsers(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return app;
        }
    }
}

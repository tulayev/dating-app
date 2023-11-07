using API.Helpers;
using API.Repositories;
using API.SignalR.Presence;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.AutoMapper;
using Services.PhotoUploadService;
using Services.TokenService;

namespace API.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

            return services;
        }
    }
}

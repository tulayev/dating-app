using API.Repository;
using API.Repository.IRepository;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.AutoMapper;
using Services.TokenService;

namespace API.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

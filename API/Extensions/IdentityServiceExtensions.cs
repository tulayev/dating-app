using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<AppRole>>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.AdminPolicy, policy => policy.RequireRole(Constants.AdminRole));
                options.AddPolicy(Constants.ModeratorPolicy, policy => policy.RequireRole(Constants.AdminRole, Constants.ModeratorRole));
            });

            return services;
        }
    }
}

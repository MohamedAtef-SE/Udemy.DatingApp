using API.Data;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection IdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentityCore<AppUser>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<DataContext>();

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var securityKey = configuration["JWTSettings:SecurityKey"] ?? throw new Exception("SecurityKey not found in appSettings.json");

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,

                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var access_token = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if(!string.IsNullOrEmpty(access_token) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = access_token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });


            services.AddAuthorizationBuilder()
                    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
                    .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator"));

            return services;
        }
    }
}

using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection IdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
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
                });


            return services;
        }
    }
}

using API.Data;
using API.Exceptions;
using API.Interfaces.Services;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection ApplicationService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            //        .ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = false; // default
            //    options.InvalidModelStateResponseFactory = actionContext =>
            //    {

            //        var errors = actionContext.ModelState.Where(prop => prop.Value.Errors is not null)
            //                                             .SelectMany(prop => prop.Value!.Errors)
            //                                             .Select(error => error.ErrorMessage)
            //                                             .ToList();
                                                         

            //        var response = new ApiValidationException(400, errors);

            //        return new BadRequestObjectResult(response);
            //    };
            //});


            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}

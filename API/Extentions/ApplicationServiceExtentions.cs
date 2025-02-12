using API.Data;
using API.Data.Repositories;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repositories;
using API.Interfaces.Services;
using API.Services;
using API.SignalR;
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

            services.AddScoped<LogUserActivity>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            services.AddScoped<ILikesRepository, LikesRepository>();

            services.AddScoped<IPhotoService, PhotoService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSignalR(configure =>
            {
                // default (30 seconds)
                configure.ClientTimeoutInterval = TimeSpan.FromSeconds(10);

                // default (15 seconds)
                configure.KeepAliveInterval = TimeSpan.FromSeconds(5);
            });

            services.AddSingleton<PresenceTracker>();

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;
        }
    }
}

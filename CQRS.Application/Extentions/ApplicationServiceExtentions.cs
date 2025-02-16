using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Application.Services;
using CQRS.Infrastructure.Interfaces;
using CQRS.Infrastructure.Interfaces.Repositories;
using CQRS.Infrastructure.Persistence;
using CQRS.Infrastructure.Persistence.Repositories;
using CQRS.Infrastructure.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Application.Extentions
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILikesRepository, LikesRepository>();

            services.AddScoped<IPhotoService, PhotoService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpContextAccessor();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyInformation).Assembly));

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

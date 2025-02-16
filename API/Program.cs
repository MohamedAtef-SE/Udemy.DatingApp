using API.Extentions;
using API.Middlewares;
using CQRS.Application.Extentions;
using CQRS.Infrastructure.SignalR;

namespace API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.ApplicationService(builder.Configuration);
            builder.Services.IdentityServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<CustomExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                               .WithOrigins("http://localhost:4200", "https://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.MapControllers();
            app.MapHub<PresenceHub>("hubs/presence"); // adding endpoint
            app.MapHub<MessageHub>("hubs/message"); // adding endpoint

            await app.SeedAsync();

            app.Run();
        }
    }
}

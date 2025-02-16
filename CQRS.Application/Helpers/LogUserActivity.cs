using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Application.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();
            if (context.HttpContext.User.Identity?.IsAuthenticated is not true) return;

            var userId = context.HttpContext.User.GetUserId();

            var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if (user is null) return;

            user.LastActive = DateTime.UtcNow;
            await unitOfWork.CompleteAsync();
        }
    }
}
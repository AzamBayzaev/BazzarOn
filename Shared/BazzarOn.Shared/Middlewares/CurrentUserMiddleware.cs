using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BazzarOn.Shared.Extensions;

namespace BazzarOn.Shared.Middlewares;

public class CurrentUserMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var currentUserAccessor = context.RequestServices.GetRequiredService<ICurrentUserAccessor>();
        using var _ = currentUserAccessor.BeginScope(context.User);

        await next(context);
    }
}
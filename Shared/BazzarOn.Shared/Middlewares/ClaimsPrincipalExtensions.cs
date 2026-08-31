using System.Security.Claims;
using BazzarOn.Mediator.Helper.Common.Models;
using BazzarOn.Mediator.Helper.Exceptions;

namespace BazzarOn.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out long userId)
    {
        userId = 0;

        if (user is null)
            return false;

        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return long.TryParse(value.AsSpan(), out userId);
    }

    public static long GetUserId(this ClaimsPrincipal user)
    {
        if (!user.TryGetUserId(out var userId))
        {
            throw new UnauthorizedException(
                new Error(
                    "Auth.UserNull",
                    "Не удалось получить идентификатор пользователя."
                )
            );
        }

        return userId;
    }
}
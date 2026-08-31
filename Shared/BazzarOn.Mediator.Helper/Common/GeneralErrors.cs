using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Common;

public static class GeneralErrors
{
    public static readonly Error Unauthorized = 
        new("General.Unauthorized", "Не удалось получить текущего пользователя.");

    public static readonly Error AccessDenied = 
        new("General.AccessDenied", "Доступ запрещен.");

    public static Error CreationError(string message) =>
        new("Create.Error", $"При добавлении возникла ошибка. {message}");

    public static Error UpdateError(string message) =>
        new("Update.Error", $"При обновлении возникла ошибка. {message}");

    public static Error DeleteError(string message) =>
        new("Delete.Error", $"При удалении возникла ошибка. {message}");
}
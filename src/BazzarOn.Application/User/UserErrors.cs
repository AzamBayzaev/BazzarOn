using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Application.User;

public static class UserErrors
{
    public static readonly Error UserAlreadyExists = new(
        "User.UserAlreadyExists",
        "User with this username already exists.");

    public static readonly Error EmailAlreadyExists = new(
        "User.EmailAlreadyExists",
        "User with this email already exists.");

    public static readonly Error EmailIsNotFound = new(
        "User.EmailIsNotFound",
        "User with this email doesn't exists.");
}
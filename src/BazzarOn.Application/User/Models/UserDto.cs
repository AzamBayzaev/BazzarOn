using BazzarOn.Domain.Enum;
using BazzarOn.Domain.ValueObjects;

namespace BazzarOn.Application.User.Models;

public class UserDto
{
    public Username? Username { get; }
    public Email? Email { get; }
    public UserRole UserRole { get; }
    public bool IsEmailVerified { get; }
    public bool IsDeleted { get; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime? DeletedAt { get; }

    public UserDto(Username? username, Email? email, UserRole userRole,
        bool isEmailVerified, bool isDeleted, DateTime createdAt,
        DateTime? updatedAt, DateTime? deletedAt)
    {
        Username = username;
        Email = email;
        UserRole = userRole;
        IsEmailVerified = isEmailVerified;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }
}
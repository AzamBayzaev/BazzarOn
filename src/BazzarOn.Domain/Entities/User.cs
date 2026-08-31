using BazzarOn.Domain.Common;
using BazzarOn.Domain.Enum;
using BazzarOn.Domain.ValueObjects;

namespace BazzarOn.Domain.Entities;

public class User : ISoftDelete
{
    public Guid Id { get; private set; }
    public Username Username { get; private set; } 
    public Email Email { get; private set; }
    public Password Password { get; private set; } 
    public UserRole UserRole { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; set; }
    
    private User() 
    {
        Username = null!;
        Email = null!;
        Password = null!;
    }

    public User(Username username, Email email, Password password, UserRole role)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        Password = password;
        UserRole = role;
        DeletedAt = null; 
        IsEmailVerified = false;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        if (IsEmailVerified) return;

        IsEmailVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(Password newPassword)
    {
        Password = newPassword;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(Username newUsername)
    {
        Username = newUsername;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted) return;

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        DeletedAt = DateTime.UtcNow;
    }
}
using BazzarOn.Application.User.Interfaces;
using BazzarOn.Application.User.Models;
using BazzarOn.Application.User.Repositories;
using BazzarOn.Application.User.Specification;
using BazzarOn.Domain.Enum;
using BazzarOn.Domain.ValueObjects;
using BazzarOn.Mediator.Helper.Exceptions;
using FluentValidation;
using MediatR;

namespace BazzarOn.Application.User.Commands;

public record RegisterUserCommand(
    string Username,
    string Email,
    string Password,
    UserRole Role
) : IRequest<(string, UserDto)>;

// ReSharper disable once UnusedType.Global
public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(30).WithMessage("Username cannot exceed 30 characters");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MaximumLength(40).WithMessage("Password cannot exceed 40 characters");
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, (string, UserDto)>
{
    private readonly IUserRepository _userRepository;
    private readonly UserMapper _userMapper;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        UserMapper userMapper, 
        IEmailVerificationService emailVerificationService,
        IIdentityService identityService)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
        _emailVerificationService = emailVerificationService;
        _identityService = identityService;
    }

    public async Task<(string, UserDto)> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var namespec = new ByUserNameSpec(command.Username);
        if (await _userRepository.AnyAsync(namespec, cancellationToken))
            throw new BusinessLogicException(UserErrors.UserAlreadyExists);

        var emailspec = new ByUserEmailSpec(command.Email);
        if (await _userRepository.AnyAsync(emailspec, cancellationToken))
            throw new BusinessLogicException(UserErrors.EmailAlreadyExists);

        var newUser = new Domain.Entities.User(
            new Username(command.Username), 
            new Email(command.Email), 
            new Password(command.Password), 
            command.Role);

        await _identityService.CreateUserAsync(
            newUser.Id, 
            command.Username, 
            command.Email, 
            command.Password, 
            cancellationToken);

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        await _emailVerificationService.SendConfirmationEmailAsync(newUser.Id, cancellationToken);

        var userDto = _userMapper.Map(newUser);
        return ("User registered successfully", userDto);
    }
}
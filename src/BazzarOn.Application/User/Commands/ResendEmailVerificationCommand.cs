using BazzarOn.Application.User.Interfaces;
using BazzarOn.Application.User.Repositories;
using BazzarOn.Application.User.Specification;
using BazzarOn.Mediator.Helper.Commands;
using BazzarOn.Mediator.Helper.Exceptions;
using FluentValidation;

namespace BazzarOn.Application.User.Commands;

public record ResendEmailVerificationCommand(string Email) : ICommand<(bool, string)>;

// ReSharper disable once UnusedType.Global
public class ResendEmailVerificationValidator : AbstractValidator<ResendEmailVerificationCommand>
{
    public ResendEmailVerificationValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email")
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters");
    }
}

public class ResendEmailVerificationCommandHandler : ICommandHandler<ResendEmailVerificationCommand, (bool, string)>
{
    private readonly IUserRepository _repository;
    private readonly IEmailVerificationService _emailVerificationService;

    public ResendEmailVerificationCommandHandler(
        IUserRepository repository,
        IEmailVerificationService emailVerificationService)
    {
        _repository = repository;
        _emailVerificationService = emailVerificationService;
    }

    public async Task<(bool, string)> Handle(
        ResendEmailVerificationCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new ByUserEmailSpec(request.Email);
        var user = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (user is null)
            throw new ResourceNotFoundException(UserErrors.EmailIsNotFound);

        await _emailVerificationService.ResendConfirmationEmailAsync(user.Id, cancellationToken);

        return (true, "Confirmation code resent successfully");
    }
}
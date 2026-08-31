using BazzarOn.Application.User.Interfaces;
using BazzarOn.Application.User.Repositories;
using BazzarOn.Application.User.Specification;
using BazzarOn.Mediator.Helper.Commands;
using BazzarOn.Mediator.Helper.Exceptions;
using FluentValidation;

namespace BazzarOn.Application.User.Commands;

public record ConfirmEmailCommand(string Email,string Code) : ICommand<(bool,string)>;

// ReSharper disable once UnusedType.Global
public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email")
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters");

        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(10).WithMessage("Code cannot exceed 10 characters");
    }
}

public class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand, (bool,string)>
{
    private readonly IUserRepository _repository;
    private readonly IEmailVerificationService _emailVerificationService;
    
    public ConfirmEmailCommandHandler(
        IUserRepository repository,
        IEmailVerificationService emailVerificationService)
    {
        _repository = repository;
        _emailVerificationService = emailVerificationService;
    }

    public async Task<(bool,string)> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var spec = new ByUserEmailSpec(request.Email);
        
        var user = await _repository.FirstOrDefaultAsync(spec,cancellationToken);
        
        if (user is null)
            throw new ResourceNotFoundException(UserErrors.EmailIsNotFound);
        
        var result = await _emailVerificationService.ConfirmEmailAsync(user.Id, request.Code,cancellationToken);

        if (!result)
            return (false,"Email verification failed");
        
        return (true,"Email verification succeeded");
    }
}
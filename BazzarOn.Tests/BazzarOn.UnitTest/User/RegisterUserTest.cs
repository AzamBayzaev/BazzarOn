using AutoFixture;
using BazzarOn.Application.User;
using BazzarOn.Application.User.Commands;
using BazzarOn.Application.User.Interfaces;
using BazzarOn.Application.User.Repositories;
using BazzarOn.Application.User.Specification;
using BazzarOn.Domain.Enum;
using BazzarOn.Mediator.Helper.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
 
namespace Test.BazzarOn.UnitTest.User;
 
public class RegisterUserTest
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IIdentityService _identityService;
    private readonly RegisterUserCommandHandler _handler;
 
    public RegisterUserTest()
    {
        var fixture = new Fixture().WithAutoNSubstitutions();
 
        _userRepository = fixture.Freeze<IUserRepository>();
        _emailVerificationService = fixture.Freeze<IEmailVerificationService>();
        _identityService = fixture.Freeze<IIdentityService>();
 
        var userMapper = new UserMapper();
 
        _handler = new RegisterUserCommandHandler(
            _userRepository,
            userMapper,
            _emailVerificationService,
            _identityService);
    }
    
    private static RegisterUserCommand CreateValidCommand(
        string username = "testuser",
        string email = "test@example.com",
        string password = "SecurePassword123!",
        UserRole role = UserRole.User)
        => new(username, email, password, role);
 
    [Fact]
    public async Task Handle_WhenUsernameAlreadyExists_ShouldThrowUserAlreadyExistsException()
    {
        var command = CreateValidCommand();
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(true);
 
        var action = () => _handler.Handle(command, CancellationToken.None);
 
        var exception = await action.Should().ThrowAsync<BusinessLogicException>();
        exception.Which.Error.Should().Be(UserErrors.UserAlreadyExists);
 
        await _userRepository.DidNotReceive().AnyAsync(Arg.Any<ByUserEmailSpec>(), Arg.Any<CancellationToken>());
        await _identityService.DidNotReceive().CreateUserAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
 
    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldThrowEmailAlreadyExistsException()
    {
        var command = CreateValidCommand();
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(false);
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(true);
 
        var action = () => _handler.Handle(command, CancellationToken.None);
 
        var exception = await action.Should().ThrowAsync<BusinessLogicException>();
        exception.Which.Error.Should().Be(UserErrors.EmailAlreadyExists);
 
        await _identityService.DidNotReceive().CreateUserAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _userRepository.DidNotReceive().AddAsync(
            Arg.Any<global::BazzarOn.Domain.Entities.User>(), Arg.Any<CancellationToken>());
    }
 
    [Fact]
    public async Task Handle_WhenDataIsValid_ShouldRegisterUserAndSendEmail()
    {
        var command = CreateValidCommand();
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(false);
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(false);
 
        Guid? createdUserId = null;
        _identityService
            .CreateUserAsync(Arg.Do<Guid>(id => createdUserId = id), command.Username, command.Email, command.Password, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
 
        var (message, userDto) = await _handler.Handle(command, CancellationToken.None);
 
        message.Should().Be("User registered successfully");
        userDto.Should().NotBeNull();
        userDto.Email.Should().Be(command.Email);
        userDto.Username.Should().Be(command.Username);
 
        await _identityService.Received(1).CreateUserAsync(
            Arg.Any<Guid>(),
            command.Username,
            command.Email,
            command.Password,
            Arg.Any<CancellationToken>());
        
        await _userRepository.Received(1).AddAsync(
            Arg.Is<global::BazzarOn.Domain.Entities.User>(u => u.Id == createdUserId),
            Arg.Any<CancellationToken>());
 
        await _userRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
 
        await _emailVerificationService.Received(1).SendConfirmationEmailAsync(
            Arg.Is<Guid>(id => id == createdUserId),
            Arg.Any<CancellationToken>());
    }
 
    [Fact]
    public async Task Handle_WhenIdentityCreationFails_ShouldNotPersistUserOrSendConfirmationEmail()
    {
        var command = CreateValidCommand();
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserNameSpec>(), Arg.Any<CancellationToken>())
            .Returns(false);
 
        _userRepository
            .AnyAsync(Arg.Any<ByUserEmailSpec>(), Arg.Any<CancellationToken>())
            .Returns(false);
 
        _identityService
            .CreateUserAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Identity creation failed"));
 
        var action = () => _handler.Handle(command, CancellationToken.None);
 
        await action.Should().ThrowAsync<InvalidOperationException>();
 
        await _userRepository.DidNotReceive().AddAsync(
            Arg.Any<global::BazzarOn.Domain.Entities.User>(), Arg.Any<CancellationToken>());
        await _userRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await _emailVerificationService.DidNotReceive().SendConfirmationEmailAsync(
            Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
}
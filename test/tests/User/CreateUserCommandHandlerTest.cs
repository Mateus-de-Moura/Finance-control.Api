using Ardalis.Result;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Handlers;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using Moq;

public class CreateUserCommandHandlerTest
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IConvertFormFileToBytes> _convertMock;

    public CreateUserCommandHandlerTest()
    {
        _authServiceMock = new Mock<IAuthService>();
        _mapperMock = new Mock<IMapper>();
        _userRepository = new Mock<IUserRepository>();
        _convertMock = new Mock<IConvertFormFileToBytes>();

        _handler = new CreateUserCommandHandler(_userRepository.Object, _mapperMock.Object, _authServiceMock.Object, _convertMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Name = "test",
            Surname = "test",
            Email = "user@test.com",
            Username = "testuser",
            Password = "123456",
            RoleId = new Guid("F39B093C-9887-4A86-BBA5-48BE3C1466E4")
        };

        _authServiceMock.Setup(x => x.UniqueEmailAndUserName(command.Email!, command.Username!))
            .ReturnsAsync(ValidationFieldsUserEnum.isValid);

        _authServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("some-refresh-token");

        _authServiceMock.Setup(x => x.GenerateJWT(command.Email!, command.Username!, It.IsAny<Guid>()))
            .Returns("jwt-token");

        var userEntity = new User { Email = command.Email, UserName = command.Username, Name = command.Name };
        var tokenViewModel = new RefreshTokenViewModel();

        _mapperMock.Setup(x => x.Map<User>(It.IsAny<CreateUserCommand>())).Returns(userEntity);
        _mapperMock.Setup(x => x.Map<RefreshTokenViewModel>(userEntity)).Returns(tokenViewModel);
        _userRepository.Setup(x => x.Create(userEntity)).ReturnsAsync(Result.Success(userEntity));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ResponseInfo);
        Assert.Equal("jwt-token", result.Value!.TokenJwt);
        Assert.Equal("some-refresh-token", userEntity.RefreshToken);
        _userRepository.Verify(x => x.Create(userEntity), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenRepositoryFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "user@test.com", Username = "testuser", Password = "123456" };

        _authServiceMock.Setup(x => x.UniqueEmailAndUserName(command.Email!, command.Username!))
            .ReturnsAsync(ValidationFieldsUserEnum.isValid);
        _mapperMock.Setup(x => x.Map<User>(It.IsAny<CreateUserCommand>())).Returns(new User());
        _userRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(Result<User>.Error());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(400, result.ResponseInfo?.HttpStatus);
        _authServiceMock.Verify(x => x.GenerateJWT(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
    }
}

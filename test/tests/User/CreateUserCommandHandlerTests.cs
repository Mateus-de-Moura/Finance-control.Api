using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Handlers;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserCommandHendler _handler;
    private readonly FinanceControlContex _context;

    public CreateUserCommandHandlerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _mapperMock = new Mock<IMapper>();

        var options = new DbContextOptionsBuilder<FinanceControlContex>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FinanceControlContex(options);

        _handler = new CreateUserCommandHendler(_context, _mapperMock.Object, _authServiceMock.Object);
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

        _authServiceMock.Setup(x => x.GenerateJWT(command.Email!, command.Username!))
            .Returns("jwt-token");

        var userEntity = new User { Email = command.Email, UserName = command.Username, Name = command.Name };
        var tokenViewModel = new RefreshTokenViewModel { TokenJwt = "jwt-token" };

        _mapperMock.Setup(x => x.Map<User>(It.IsAny<CreateUserCommand>())).Returns(userEntity);
        _mapperMock.Setup(x => x.Map<RefreshTokenViewModel>(userEntity)).Returns(tokenViewModel);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ResponseInfo);
        Assert.Equal("jwt-token", result.Value!.TokenJwt);
        Assert.Equal(command.Email, userEntity.Email);

        var savedUser = await _context.Users.FirstOrDefaultAsync();
        Assert.NotNull(savedUser);
    }
}

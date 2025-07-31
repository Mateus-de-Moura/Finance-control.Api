using Ardalis.Result;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Handlers;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Interfaces.Repositories;
using Moq;

namespace tests.User
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateUserCommandHendler _handler;
        private readonly Mock<IConvertFormFileToBytes> _convert;
        private readonly Mock<IUserRepository> _userRepository;

        public UpdateUserCommandHandlerTest()
        {
            _mapperMock = new Mock<IMapper>();
            _convert = new Mock<IConvertFormFileToBytes>();
            _userRepository = new Mock<IUserRepository>();

            _handler = new UpdateUserCommandHendler(_mapperMock.Object, _convert.Object, _userRepository.Object);
        }

        [Fact]
        public async Task Update_User_Name_Surname_And_Username()
        {
            var command = new UpdateUserCommand
            {
                Active = false,
                Id = new Guid("c3d2251f-1e0b-42b6-8868-75d03046460c"),
                Name = "Admin update",
                Surname = "update",
                Email = "admin@admin.com",
                Username = "Admin update",
                RoleId = new Guid("F39B093C-9887-4A86-BBA5-48BE3C1466E4")
            };

            var userEntity = new finance_control.Domain.Entity.User
            {
                Id = command.Id,
                Name = command.Name,
                Surname = command.Surname,
                Email = command.Email,
                UserName = command.Username,
                AppRoleId = command.RoleId
            };

            _mapperMock.Setup(x => x.Map<finance_control.Domain.Entity.User>(It.IsAny<UpdateUserCommand>())).Returns(userEntity);

            _userRepository.Setup(repo => repo.Update(It.IsAny<finance_control.Domain.Entity.User>())).ReturnsAsync(Result.Success(userEntity));

            var result = await _handler.Handle(command, default);


            Assert.True(result.Value != null);
            Assert.False(result.Value.Active);
            Assert.Equal("Admin update", result.Value.Name);
            Assert.Equal("update", result.Value.Surname);
            Assert.Equal("admin@admin.com", result.Value.Email);
            Assert.Equal("Admin update", result.Value.UserName);
            Assert.Equal(command.RoleId, result.Value.AppRoleId);

            // Verificar se o método Update do repositório foi chamado
            _userRepository.Verify(repo => repo.Update(It.IsAny<finance_control.Domain.Entity.User>()), Times.Once);
        }

        [Fact]
        public async Task Update_User_Should_Return_Fail_When_User_Not_Found()
        {
            var command = new UpdateUserCommand
            {
                Active = false,
                Id = new Guid("c3d2251f-1e0b-42b6-8868-75d03046460c"),
                Name = "Admin update",
                Surname = "update",
                Email = "admin@admin.com",
                Username = "Admin update",
                RoleId = new Guid("F39B093C-9887-4A86-BBA5-48BE3C1466E4")
            };

            _mapperMock.Setup(m => m.Map<finance_control.Domain.Entity.User>(It.IsAny<UpdateUserCommand>())).Returns((finance_control.Domain.Entity.User)null);

            var result = await _handler.Handle(command, default);

            Assert.False(result.Value != null);
            Assert.Equal("Erro ao atualizar usuário", result.ResponseInfo.ErrorDescription);

            _userRepository.Verify(repo => repo.Update(It.IsAny<finance_control.Domain.Entity.User>()), Times.Never);
        }
    }
}


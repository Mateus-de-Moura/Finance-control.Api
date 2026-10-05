using Ardalis.Result;
using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handlers;
using finance_control.Domain.Interfaces.Repositories;
using Moq;

namespace tests.Expense
{
    public class DeleteExpenseHandlerTest
    {
        private readonly Mock<IExpenseRepository> _expenseRepository;
        private readonly DeleteExpenseHandler _handler;
        public DeleteExpenseHandlerTest()
        {
            _expenseRepository = new Mock<IExpenseRepository>();
            _handler = new DeleteExpenseHandler(_expenseRepository.Object);
        }

        [Fact]
        public async Task ExpenseExits_Delete_Success()
        {
            //Arrange
            var expenseId = Guid.NewGuid();
            _expenseRepository.Setup(x => x.Delete(expenseId)).ReturnsAsync(Result.Success(true));

            var command = new DeleteExpenseCommand (expenseId);

            //Act
            var result = await _handler.Handle(command, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ResponseInfo);
            Assert.NotNull(result.Value);
            _expenseRepository.Verify(x => x.Delete(expenseId), Times.Once);
        }

        [Fact]
        public async Task ExpenseDoesntExists_Delete_Error()
        {
            //Arrange
            _expenseRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(Result<bool>.NotFound());

            var command = new DeleteExpenseCommand(Guid.NewGuid());

            //Act
            var result = await _handler.Handle(command,CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.ResponseInfo?.HttpStatus);
            Assert.Null(result.Value);
            Assert.Equal("não foi possivel localizar um registro com o Id informado, tente novamente", result.ResponseInfo?.ErrorDescription);
        }
    }
}

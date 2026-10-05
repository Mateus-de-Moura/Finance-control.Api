using Ardalis.Result;
using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handlers;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using Moq;

namespace tests.Expense
{
    public class UpdateExpenseHandlerTest
    {
        private readonly Mock<IExpenseRepository> _expenseRepository;
        private readonly UpdateExpenseHandler _handler;
        private readonly Mock<IConvertFormFileToBytes> _convertMock;
        public UpdateExpenseHandlerTest()
        {
            _convertMock = new Mock<IConvertFormFileToBytes>();
            _expenseRepository = new Mock<IExpenseRepository>();

            _expenseRepository.Setup(x => x.Update(It.IsAny<Expenses>()))
                .ReturnsAsync((Expenses e) => Result.Success(e));

            _handler = new UpdateExpenseHandler(_expenseRepository.Object, _convertMock.Object);
        }

        [Fact]
        public async Task ExpenseExist_Update_Success()
        {
            //Arrange
            var expenseId = Guid.NewGuid();
            var existsExpense = new Expenses
            {
                Id = expenseId,
                Value = 1000,
                DueDate = DateTime.Parse("12/12/2025"),
                UserId = Guid.Parse("C3D2251F1E0B42B6886875D03046460C"),
                CategoryId = Guid.Parse("D7F14F6C5B1D4C68B2E78C236CBA4A9F"),
                IsDeleted = false
            };

            _expenseRepository.Setup(x => x.GetById(expenseId)).ReturnsAsync(Result.Success(existsExpense));

            var command = new UpdateExpenseCommand
            {
                IdExpense = expenseId,
                Value = 2500,
                DueDate = DateTime.Parse("01/02/2025"),
                Status = InvoicesStatus.Pago
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ResponseInfo);
            Assert.Equal(2500, result.Value?.Value);
            Assert.Equal(DateTime.Parse("01/02/2025"), result.Value?.DueDate);
            Assert.Equal(InvoicesStatus.Pago, result.Value?.Status);
            _expenseRepository.Verify(x => x.Update(existsExpense), Times.Once);
        }

        [Fact]
        public async Task ExpenseDoesNotExits_Update_Error()
        {
            _expenseRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(Result<Expenses>.NotFound());

            var command = new UpdateExpenseCommand
            {
                IdExpense = Guid.NewGuid(),
                Value = 2500,
                DueDate = DateTime.Parse("01/02/2025"),
                Status = InvoicesStatus.Pago
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.ResponseInfo?.HttpStatus);
            Assert.Null(result.Value);
            Assert.Equal("não foi possivel localizar um registro com o Id informado, tente novamente", result.ResponseInfo?.ErrorDescription);
            _expenseRepository.Verify(x => x.Update(It.IsAny<Expenses>()), Times.Never);
        }
    }
}

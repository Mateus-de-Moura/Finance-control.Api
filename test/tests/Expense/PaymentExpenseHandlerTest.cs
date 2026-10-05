using Ardalis.Result;
using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handlers;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using Moq;

namespace tests.Expense
{
    public class PaymentExpenseHandlerTest
    {
        private readonly Mock<IExpenseRepository> _expenseRepository;
        private readonly PaymentExpenseHandler _handler;
        public PaymentExpenseHandlerTest()
        {
            _expenseRepository = new Mock<IExpenseRepository>();
            _expenseRepository.Setup(x => x.Update(It.IsAny<Expenses>()))
                .ReturnsAsync((Expenses e) => Result.Success(e));

            _handler = new PaymentExpenseHandler(_expenseRepository.Object);
        }

        [Fact]
        public async Task PaymentItsOk_Payment_Success()
        {
            //Arrange
            var expenseId = Guid.NewGuid();
            var userId = Guid.Parse("C3D2251F1E0B42B6886875D03046460C");
            var existsExpense = new Expenses
            {
                Id = expenseId,
                Description = "Despesa de teste",
                Value = 1000,
                DueDate = DateTime.Parse("12/12/2025"),
                UserId = userId,
                CategoryId = Guid.Parse("D7F14F6C5B1D4C68B2E78C236CBA4A9F"),
                IsDeleted = false
            };
            _expenseRepository.Setup(x => x.GetById(expenseId)).ReturnsAsync(Result.Success(existsExpense));

            var command = new PaymentExpenseCommand
            {
                IdExpense = expenseId,
                UserId = userId,
                PaymentDate = DateTime.Today,
                Value = 50
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(result.Value);
            Assert.Null(result.ResponseInfo);

            Assert.Equal(InvoicesStatus.Pago, existsExpense.Status);
            Assert.Equal(DateTime.Today, existsExpense.PaymentDate);
            Assert.Equal(50, existsExpense.Value);
            _expenseRepository.Verify(x => x.Update(existsExpense), Times.Once);
        }

        [Fact]
        public async Task ExpenseFromAnotherUser_Payment_Error()
        {
            //Arrange
            var expenseId = Guid.NewGuid();
            var existsExpense = new Expenses
            {
                Id = expenseId,
                Description = "Despesa de outro usuário",
                Value = 1000,
                UserId = Guid.NewGuid(),
            };
            _expenseRepository.Setup(x => x.GetById(expenseId)).ReturnsAsync(Result.Success(existsExpense));

            var command = new PaymentExpenseCommand
            {
                IdExpense = expenseId,
                UserId = Guid.NewGuid(),
                PaymentDate = DateTime.Today,
                Value = 50
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(404, result.ResponseInfo?.HttpStatus);
            _expenseRepository.Verify(x => x.Update(It.IsAny<Expenses>()), Times.Never);
        }

        [Fact]
        public async Task ExpenseDoesNotExists_Paymente_Error()
        {
            //Arrange
            _expenseRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(Result<Expenses>.NotFound());

            var command = new PaymentExpenseCommand
            {
                IdExpense = Guid.NewGuid(),
                PaymentDate = DateTime.Today,
                Value = 50
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(result.Value);
            Assert.NotNull(result.ResponseInfo);
            Assert.Equal("Despesa não encontrada ou não existe, tente novamente.",result.ResponseInfo.ErrorDescription);
            Assert.Equal(404, result.ResponseInfo?.HttpStatus);
        }
    }
}

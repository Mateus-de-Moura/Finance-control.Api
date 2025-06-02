using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handler;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace tests.Expense
{
    public class UpdateExpenseHandlerTest
    {
        private readonly FinanceControlContex _context;
        private readonly UpdateExpenseHandler _handler;
        public UpdateExpenseHandlerTest()
        {
            var options = new DbContextOptionsBuilder<FinanceControlContex>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new FinanceControlContex(options);
            _handler = new UpdateExpenseHandler(_context);
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

            await _context.Expenses.AddAsync(existsExpense);
            await _context.SaveChangesAsync();

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


        }

        [Fact]
        public async Task ExpenseDoesNotExits_Update_Error()
        {
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

        }
    }
}

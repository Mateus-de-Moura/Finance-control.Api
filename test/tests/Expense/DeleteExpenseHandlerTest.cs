using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handler;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace tests.Expense
{
    public class DeleteExpenseHandlerTest
    {
        private readonly FinanceControlContex _context;
        private readonly DeleteExpenseHandler _handler;
        public DeleteExpenseHandlerTest()
        {
            var options = new DbContextOptionsBuilder<FinanceControlContex>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new FinanceControlContex(options);
            _handler = new DeleteExpenseHandler(_context);
        }

        [Fact]
        public async Task ExpenseExits_Delete_Success()
        {
            //Arrange
            var expense = new Expenses
            {
                Id = Guid.NewGuid(),
                Value = 1000,
                DueDate = DateTime.Parse("12/12/2025"),
                UserId = Guid.Parse("C3D2251F1E0B42B6886875D03046460C"),
                CategoryId = Guid.Parse("D7F14F6C5B1D4C68B2E78C236CBA4A9F"),
                IsDeleted = false
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            var command = new DeleteExpenseCommand (expense.Id);

            //Act
            var result = await _handler.Handle(command, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ResponseInfo);
            Assert.NotNull(result.Value);
            Assert.True(result.Value.IsDeleted);
        }

        [Fact]
        public async Task ExpenseDoesntExists_Delete_Error()
        {
            //Arrange
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

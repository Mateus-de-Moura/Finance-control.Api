using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handler;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace tests.Expense
{
    public class CreateExpenseHandlerTest
    {
        private readonly FinanceControlContex _context;
        private readonly CreateExpenseHandler _handler;
        private readonly Mock<IConvertFormFileToBytes> _convertMock;
        private readonly Mock<IExpenseRepository> _expenseRepository;
        public CreateExpenseHandlerTest()
        {
            var options = new DbContextOptionsBuilder<FinanceControlContex>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new FinanceControlContex(options);
            _convertMock = new Mock<IConvertFormFileToBytes>();
            _expenseRepository = new Mock<IExpenseRepository>();

            _handler = new CreateExpenseHandler(_expenseRepository.Object, _convertMock.Object);
        }

        [Fact]
        public async Task InputDataAreOk_Create_Success()
        {
            //Arrange
            var command = new CreateExpenseCommand
            {
                Value = 1000,
                DueDate = DateTime.Parse("12/12/2025"),
                UserId = Guid.Parse("C3D2251F1E0B42B6886875D03046460C"),
                CategoryId = Guid.Parse("D7F14F6C5B1D4C68B2E78C236CBA4A9F")
            };

            //Act
            var result = await _handler.Handle(command,CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.ResponseInfo);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Value, result.Value.Value);
            Assert.Equal(command.DueDate, result.Value.DueDate);
            Assert.Equal(command.UserId, result.Value.UserId);
            Assert.Equal(command.CategoryId, result.Value.CategoryId);

            var savedExpense = await _context.Set<Expenses>().FirstOrDefaultAsync(e => e.UserId == command.UserId);
            Assert.NotNull(savedExpense);
        }
    }
}

using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Handler;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace tests.Expense
{
    public class PaymentExpenseHandlerTest
    {

        private readonly FinanceControlContex _context;
        private readonly PaymentExpenseHandler _handler;
        public PaymentExpenseHandlerTest()
        {
            var options = new DbContextOptionsBuilder<FinanceControlContex>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new FinanceControlContex(options);
            _handler = new PaymentExpenseHandler(_context);
        }
        [Fact]
        public async Task PaymentItsOk_Payment_Success()
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

            var command = new PaymentExpenseCommand
            {
                IdExtense = expenseId,
                ProofFile = null
            };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(result.Value);
            Assert.Null(result.ResponseInfo);

            var updatedExpense = await _context.Expenses.FindAsync(expenseId);
            Assert.Equal(InvoicesStatus.Pago, updatedExpense.Status);

        }

        [Fact]
        public async Task ExpenseDoesNotExists_Paymente_Error()
        {
            //Arrange
            var expenseId = Guid.NewGuid();

            var command = new PaymentExpenseCommand
            {
                IdExtense = expenseId,
                ProofFile = null
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

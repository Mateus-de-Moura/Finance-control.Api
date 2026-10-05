using Ardalis.Result;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class ExpenseRepository(FinanceControlContex financeControlContex) : IExpenseRepository
    {
        private readonly FinanceControlContex _context = financeControlContex;
        public async Task<Result<Expenses>> Create(Expenses expense)
        {
            if (expense is null)
                return Result<Expenses>.Error("Preencha todos os campos e tente novamente.");

             await _context.AddAsync(expense);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result<Expenses>.Success(expense) :
                Result<Expenses>.Error("Ocorreu um erro ao salvar os dados, tente novamente.");
        }

        public IQueryable<Expenses> GetExpensesFilter()
        {
            return _context.Expenses.AsNoTracking();
        }

        public async Task<Result<Expenses>> GetById(Guid id)
        {
            var expense = await _context.Expenses
                .Include(e => e.ExpensesComprovant)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));

            return expense != null ?
                Result.Success(expense) :
                Result.NotFound();
        }

        public async Task<Result<Expenses>> Update(Expenses expense)
        {
            _context.Entry(expense).State = EntityState.Modified;

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result.Success(expense) :
                Result.Error("Nenhuma linha foi afetada. A operação não pôde ser concluída.");
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            var rowsAffected = await _context.Expenses.Where(e => e.Id.Equals(id))
                .ExecuteUpdateAsync(x => x.SetProperty(e => e.Active, false)
                .SetProperty(e => e.IsDeleted, true));

            return rowsAffected > 0 ?
                Result.Success(true) :
                Result.NotFound();
        }
    }
}

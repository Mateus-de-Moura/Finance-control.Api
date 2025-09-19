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
    }
}

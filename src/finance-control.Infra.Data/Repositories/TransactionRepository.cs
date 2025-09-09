using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class TransactionRepository(FinanceControlContex context) : ITransactionsRepository
    {
        private readonly FinanceControlContex _context = context;
        public async Task<Result<Transactions>> AddTransaction(Transactions transaction)
        {
            if (transaction == null)
                return Result.Error("Preencha todos os campos e tente novamente");

            await _context.AddAsync(transaction);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result.Success(transaction) :
                Result.Error("");
        }

        public async Task<Result<Transactions>> GetById(Guid Id)
        {
            var transaction = await _context.Transactions.Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync();

            return transaction != null ?
                Result.Success(transaction) :
                Result.Error("");   
        }

        public async Task<Result<List<Transactions>>> GetRecentTransactions(Guid userId)
        {
            var transactions = await _context.Transactions
                .Where(x => x.UserId.Equals(userId))
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.TransactionDate)
                .Take(5)
                .ToListAsync();

            return Result.Success(transactions);
        }

        public async Task<Result<Transactions>> UpdateTransaction(Transactions transaction)
        {
            var result = await _context.Transactions.FirstOrDefaultAsync(x => x.Id.Equals(transaction.Id));

            if (result == null)
                return Result.Error("Nenhum registro localizado");

            _context.Entry(result).CurrentValues.SetValues(transaction);
            _context.Entry(result).State = EntityState.Modified;

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
               Result.Success(transaction) :
               Result.Error("");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;

namespace finance_control.Infra.Data.Repositories
{
    public class TransactionRepository(FinanceControlContex context) : ITransactionsRepository
    {
        private readonly FinanceControlContex _contex = context;
        public async Task<Result<Transactions>> AddTransaction(Transactions transaction)
        {
            if (transaction == null)
                return Result.Error("Preencha todos os campos e tente novamente");

            await _contex.AddAsync(transaction);

            var rowsAffected = await _contex.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result.Success(transaction) :
                Result.Error("");
        }
    }
}

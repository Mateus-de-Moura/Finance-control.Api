using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetRecentTransactiosHandler(ITransactionsRepository transactions) : IRequestHandler<GetRecentTransactionsQuery, ResponseBase<List<Transactions>>>
    {
        private readonly ITransactionsRepository _transactions = transactions;
        public async Task<ResponseBase<List<Transactions>>> Handle(GetRecentTransactionsQuery request, CancellationToken cancellationToken)
        {          
            var recentTransactions = await _transactions.GetRecentTransactions(request.UserId);
            return ResponseBase<List<Transactions>>.Success(recentTransactions.Value);
           
        }
    }
}

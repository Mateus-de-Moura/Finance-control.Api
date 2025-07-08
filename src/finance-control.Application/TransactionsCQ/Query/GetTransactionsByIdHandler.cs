using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetTransactionsByIdHandler(ITransactionsRepository transactionsRepository) : IRequestHandler<GetTransactionByIdQuery, ResponseBase<Transactions>>
    {
        private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;
        public async Task<ResponseBase<Transactions>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return ResponseBase<Transactions>.Fail("Erro ao localizar registros", "Tente novamente", 404);

            var response = await _transactionsRepository.GetById(request.Id);

            return response.IsSuccess ?
                ResponseBase<Transactions>.Success(response.Value) :
                ResponseBase<Transactions>.Fail("Erro ao localizar registros", "Tente novamente", 404);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.TransactionsCQ.Command;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Handler
{
    public class AddTransactionHandler(ITransactionsRepository transactionsRepository) : IRequestHandler<AddTransactionsCommand, ResponseBase<Transactions>>
    {     
        private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;
         async Task<ResponseBase<Transactions>> IRequestHandler<AddTransactionsCommand, ResponseBase<Transactions>>.Handle(AddTransactionsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return ResponseBase<Transactions>.Fail("Erro ao cadastrar transacao.", "Preencha todos os campos e tente novamente", 404);

            var result = await _transactionsRepository.AddTransaction(new Transactions
            {
                Active = request.Active,
                TransactionDate = request.TransactionDate,
                Type = request.Type,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Value = request.Value,
                PaymentMethod = request.PaymentMethod,
                Status = request.Status,
                Observation = request.Observation,
                UserId = request.UserId,
            });

            return result.IsSuccess ?
                ResponseBase<Transactions>.Success(result.Value) :
                ResponseBase<Transactions>.Fail("Erro ao cadastrar","Tente Novamente", 400);
        }
    }
}

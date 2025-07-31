using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using AutoMapper;
using finance_control.Application.Response;
using finance_control.Application.TransactionsCQ.Command;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Handler
{
    public class UpdateTransactionHendler(ITransactionsRepository transactionsRepository, IMapper mapper) : IRequestHandler<UpdateTransactionCommand, ResponseBase<Transactions>>
    {
        private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<Transactions>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return ResponseBase<Transactions>.Fail("Erro ao atualizar transacao.", "Preencha todos os campos e tente novamente", 404);

            var response = await _transactionsRepository.UpdateTransaction(_mapper.Map<Transactions>(request));

            return response.IsSuccess ?
                ResponseBase<Transactions>.Success(response) :
                ResponseBase<Transactions>.Fail("Erro", "Não foi possivel atualizar a entidade", 404);
            
        }
    }
}

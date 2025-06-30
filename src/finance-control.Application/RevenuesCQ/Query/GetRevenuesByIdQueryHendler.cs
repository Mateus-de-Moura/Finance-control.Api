using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Queries
{
    public class GetRevenuesByIdQueryHendler(IRevenuesRepository revenuesRepository) : IRequestHandler<GetRevenuesByIdQuery, ResponseBase<Revenues>>
    {      
        private readonly IRevenuesRepository _revenuesRepository = revenuesRepository;
        public async Task<ResponseBase<Revenues>> Handle(GetRevenuesByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<Revenues>.Fail("Id inválido", "Informe um Id válido", 400);

            var result = await _revenuesRepository.GetRevenuesById(request.Id);

            return result.IsSuccess  ?
                ResponseBase<Revenues>.Success(result.Value) :
                ResponseBase<Revenues>.Fail("Erro ao buscar registro", "Não foi encontrado nenhum registro com o Id informado", 404);
        }
    }
}

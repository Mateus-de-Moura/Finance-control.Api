using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Queries
{
    public class GetRevenuesByIdQueryHendler(FinanceControlContex context) : IRequestHandler<GetRevenuesByIdQuery, ResponseBase<Revenues>>
    {
        private readonly FinanceControlContex _context = context;
        public async Task<ResponseBase<Revenues>> Handle(GetRevenuesByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<Revenues>.Fail("Id inválido", "Informe um Id válido", 400);

            var revenue = await _context.Revenues
                .Where(x => x.Id.Equals(request.Id))
                .FirstOrDefaultAsync();

            return revenue != null ?
                ResponseBase<Revenues>.Success(revenue) :
                ResponseBase<Revenues>.Fail("Erro ao buscar registro", "Não foi encontrado nenhum registro com o Id informado", 404);
        }
    }
}

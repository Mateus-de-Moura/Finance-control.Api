
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Hendlers
{
    public class GetPagedRevenuesCommandHendler(FinanceControlContex context) : IRequestHandler<GetPagedRevenuesCommand, ResponseBase<PaginatedList<Revenues>>>
    {
        private readonly FinanceControlContex _context = context;
        public async Task<ResponseBase<PaginatedList<Revenues>>> Handle(GetPagedRevenuesCommand request, CancellationToken cancellationToken)
        {

            var queryable =  _context.Revenues.AsNoTracking()
                .Where(x => x.Active).AsQueryable();


            if (!string.IsNullOrEmpty(request.Description))
            {
                queryable = queryable.Where(x => x.Description.Contains(request.Description));
            };

            var response = await  queryable.PaginatedListAsync(request.PageNumber, request.PageSize);


            if (response == null)
            {
                return new ResponseBase<PaginatedList<Revenues>>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Falha ao buscar dados",
                        ErrorDescription = "Nenhum item localizado",
                        HttpStatus = 404
                    },
                    Value = null,
                };
            }


            return new ResponseBase<PaginatedList<Revenues>>
            {
                ResponseInfo = null,
                Value = response
            };

        }
    }
}

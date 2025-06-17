using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.ViewModels;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Queries
{
    public class GetPagedRevenuesQueryHendler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetPagedRevenuesQuery, ResponseBase<PaginatedList<RevenuesViewModel>>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<PaginatedList<RevenuesViewModel>>> Handle(GetPagedRevenuesQuery request, CancellationToken cancellationToken)
        {

            if (request.StartDate == null && request.EndDate == null)
            {
                var now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                request.StartDate = firstDayOfMonth;
                request.EndDate = lastDayOfMonth;
            }

            var queryable =  _context.Revenues
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.UserId.Equals(request.UserId)).AsQueryable();

            if (!string.IsNullOrEmpty(request.Description))
            {
                queryable = queryable.Where(x => x.Description.Contains(request.Description));
            };

            if (request.StartDate != null)
            {
                queryable = queryable.Where(x => x.Date >= request.StartDate);
            }

            if (request.EndDate != null)
            {
                queryable = queryable.Where(x => x.Date <= request.EndDate);
            }

            var response = await  queryable
                .Select(x => _mapper.Map<RevenuesViewModel>(x))
                .PaginatedListAsync(request.PageNumber, request.PageSize);


            if (response == null)
            {
                return new ResponseBase<PaginatedList<RevenuesViewModel>>
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


            return new ResponseBase<PaginatedList<RevenuesViewModel>>
            {
                ResponseInfo = null,
                Value = response
            };

        }
    }
}

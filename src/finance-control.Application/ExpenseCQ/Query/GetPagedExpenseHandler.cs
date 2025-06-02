using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetPagedExpenseHandler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetPagedExpenseQuery, ResponseBase<PaginatedList<ExpenseViewModel?>>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<PaginatedList<ExpenseViewModel?>>> Handle(GetPagedExpenseQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.Expenses.AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.User)
                .AsQueryable();

            if (request.UserId.HasValue)
                queryable = queryable.Where(x => x.UserId == request.UserId.Value);

            if (request.CategoryId.HasValue)
                queryable = queryable.Where(x => x.CategoryId == request.CategoryId.Value);

            if (request.StartDate.HasValue)
                queryable = queryable.Where(x => x.DueDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                queryable = queryable.Where(x => x.DueDate <= request.EndDate.Value);

            if (request.Status.HasValue)
                queryable = queryable.Where(x => x.Status == request.Status.Value);

            var paginatedList = await queryable
                .Select(x => _mapper.Map<ExpenseViewModel>(x))
               .PaginatedListAsync(request.PageNumber, request.PageSize);

            if (paginatedList == null)
            {
                return new ResponseBase<PaginatedList<ExpenseViewModel?>>
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


            return new ResponseBase<PaginatedList<ExpenseViewModel?>>
            {
                ResponseInfo = null,
                Value = paginatedList,
            };
        }
    }
}

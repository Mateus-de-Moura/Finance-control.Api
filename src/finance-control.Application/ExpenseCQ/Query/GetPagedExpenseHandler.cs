using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetPagedExpenseHandler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetPagedExpenseQuery, ResponseBase<PaginatedList<ExpenseViewModel?>>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<PaginatedList<ExpenseViewModel>>> Handle(GetPagedExpenseQuery request, CancellationToken cancellationToken)
        {
            //Define sempre o mes atual  caso  não seja filtrado no front
            if (request.StartDate == null && request.EndDate == null)
            {
                var now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                request.StartDate = firstDayOfMonth;
                request.EndDate = lastDayOfMonth;
            }

            var queryable = _context.Expenses.AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.UserId.Equals(request.UserId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Description))
                queryable = queryable.Where(x => x.Description.Contains(request.Description));

            if (request.UserId is not null)
                queryable = queryable.Where(x => x.UserId.Equals(request.UserId));

            if (request.CategoryId is not null)
                queryable = queryable.Where(x => x.CategoryId.Equals(request.CategoryId));

            if (request.StartDate is not null)
                queryable = queryable.Where(x => x.DueDate >= request.StartDate);

            if (request.EndDate is not null)
                queryable = queryable.Where(x => x.DueDate <= request.EndDate);

            if (request.Status is not null)
                queryable = queryable.Where(x => x.Status.Equals(request.Status));

            var paginatedList = await queryable
                .Select(x => _mapper.Map<ExpenseViewModel>(x))
               .PaginatedListAsync(request.PageNumber, request.PageSize);

            if (paginatedList == null)
                return ResponseBase<PaginatedList<ExpenseViewModel>>.Fail("Falha ao buscar dados", "Nenhum item localizado", 404);

            return ResponseBase<PaginatedList<ExpenseViewModel>>.Success(paginatedList);            
        }
    }
}

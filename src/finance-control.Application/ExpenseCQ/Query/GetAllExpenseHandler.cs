using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Infra.Data;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetAllExpenseHandler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetAllExpenseQuery, ResponseBase<PaginatedList<ExpenseViewModel>>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<PaginatedList<ExpenseViewModel>>> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
        {
            var expenses = _context.Expenses
                .Where(e => !e.IsDeleted)
                .AsQueryable();


            if (request.StartDate.HasValue)
                expenses = expenses.Where(e => e.DueDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                expenses = expenses.Where(e => e.DueDate <= request.EndDate.Value);

            if (request.Status.HasValue)
                expenses = expenses.Where(e => e.Status.Equals(request.Status));


            var paginatedList = await expenses
                .Select(x => _mapper.Map<ExpenseViewModel>(x))
               .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new ResponseBase<PaginatedList<ExpenseViewModel>>
            {
                ResponseInfo = null,
                Value = paginatedList,

            };
        }
    }
}

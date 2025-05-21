using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetPagedExpenseQuery : IRequest<ResponseBase<PaginatedList<ExpenseViewModel?>>>
    {
        public int PageNumber = 1;
        public int PageSize = 10;
        public Guid? UserId { get; set; } 

        public Guid? CategoryId { get; set; }

        public DateTime? StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 

        public InvoicesStatus? Status { get; set; }
    }
}

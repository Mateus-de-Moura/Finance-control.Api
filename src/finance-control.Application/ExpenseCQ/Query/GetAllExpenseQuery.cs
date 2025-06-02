using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.ViewModels;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetAllExpenseQuery :  IRequest<ResponseBase<PaginatedList<ExpenseViewModel>>>
    {
        public GetAllExpenseQuery(DateTime? startDate, DateTime? endDate, InvoicesStatus? status)
        {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public InvoicesStatus? Status { get; set; }

        public int PageNumber = 1;
        public int PageSize = 10;

    }
}

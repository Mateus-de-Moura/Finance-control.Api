using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Common.Models;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.ViewModels;
using finance_control.Application.TransactionsCQ.ViewModels;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetPagedTransactionsQuey: IRequest<ResponseBase<PaginatedList<TransactionsViewModel>>>
    {
        public Guid UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

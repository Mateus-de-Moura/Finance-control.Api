using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetTransactionByIdQuery : IRequest<ResponseBase<Transactions>>
    {
        public Guid Id { get; set; }
    }
}

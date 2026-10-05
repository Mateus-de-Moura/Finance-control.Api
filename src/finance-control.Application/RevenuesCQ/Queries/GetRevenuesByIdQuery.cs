using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.RevenuesCQ.Queries
{
    public class GetRevenuesByIdQuery : IRequest<ResponseBase<Revenues>>
    {
        public Guid Id { get; set; }
    }
}

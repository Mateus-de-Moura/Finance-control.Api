using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.RevenuesCQ.Commands
{
    public class CreateRevenueCommand : IRequest<ResponseBase<Revenues>>
    {
        public bool Active { get; set; }
        public string? Description { get; set; }

        public decimal Value { get; set; }

        public DateTime? Date { get; set; }

        public Guid CategoryId { get; set; }
    }
}

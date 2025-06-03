using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using MediatR;

namespace finance_control.Application.NotifyCQ.Query
{
    public class GetQuantityNotifyByUser : IRequest<ResponseBase<int>>
    {
        public Guid UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.DashBoardCQ.ViewModels;
using finance_control.Application.Response;
using MediatR;

namespace finance_control.Application.DashBoardCQ.Query
{
    public class DashBoardQuery : IRequest<ResponseBase<DashboardViewModel>>
    {
        public Guid UserId { get; set; }
    }
}

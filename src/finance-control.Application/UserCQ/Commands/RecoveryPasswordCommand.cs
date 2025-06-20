using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using MediatR;

namespace finance_control.Application.UserCQ.Commands
{
    public class RecoveryPasswordCommand : IRequest<ResponseBase<bool>>
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}

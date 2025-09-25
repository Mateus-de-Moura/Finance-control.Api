using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;

namespace finance_control.Application.UserCQ.Query
{
    public class GetUserAuthQuery : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string UserNameOrEmailAddress { get; set; }
    }
}

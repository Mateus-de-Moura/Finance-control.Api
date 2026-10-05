using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;

namespace finance_control.Application.UserCQ.Commands
{
    public class LoginGithubCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string Code { get; set; } = string.Empty;
    }
}

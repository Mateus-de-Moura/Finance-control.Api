using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.UserCQ.Commands
{
    public record LoginUserCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ip {  get; set; }
    }
}

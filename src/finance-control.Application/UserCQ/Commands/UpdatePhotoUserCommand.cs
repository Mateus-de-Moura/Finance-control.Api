using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.UserCQ.Commands
{
    public class UpdatePhotoUserCommand : IRequest<ResponseBase<User>>
    {
        public string EmailUser { get; set; }
        public IFormFile Photo {  get; set; }
    }
}

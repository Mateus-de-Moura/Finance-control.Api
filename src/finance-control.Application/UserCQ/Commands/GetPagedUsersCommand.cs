using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Common.Models;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.UserCQ.Commands
{
    public class GetPagedUsersCommand : IRequest<ResponseBase<PaginatedList<UserViewModel?>>>
    {
        public int PageNumber = 1;
        public int PageSize = 10;
        public string Name = string.Empty;
    }
}

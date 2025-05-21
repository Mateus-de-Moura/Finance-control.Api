using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Queries
{
    public class GetRolesToUserCommandHandler(FinanceControlContex contex) : IRequestHandler<GetRolesToUserCommand, ResponseBase<List<AppRole>>>
    {
        private readonly FinanceControlContex _context = contex;
        public async Task<ResponseBase<List<AppRole>>> Handle(GetRolesToUserCommand request, CancellationToken cancellationToken)
        {
           var roles =  await _context.AppRole.ToListAsync();

            return  ResponseBase<List<AppRole>>.Success(roles);            
        }
    }
}

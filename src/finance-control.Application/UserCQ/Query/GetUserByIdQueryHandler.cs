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
    public class GetUserByIdQueryHandler(FinanceControlContex contex) : IRequestHandler<GetUserByIdQuery, ResponseBase<User>>
    {
        private readonly FinanceControlContex _contex = contex;
        public async Task<ResponseBase<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<User>.Fail("Id inválido", "Informe um Guid válido", 404);             

            var user = await _contex.Users
                 .Where(x => x.Id.Equals(request.Id))
                 .FirstOrDefaultAsync();

            return user != null ?
                ResponseBase<User>.Success(user) :
                ResponseBase<User>.Fail("Id inválido", "Informe um Guid válido", 404);              
        }
    }
}

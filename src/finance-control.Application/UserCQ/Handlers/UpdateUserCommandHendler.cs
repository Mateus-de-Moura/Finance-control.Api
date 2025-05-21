using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Handlers
{
    public class UpdateUserCommandHendler(FinanceControlContex contex, IConvertFormFileToBytes convert) : IRequestHandler<UpdateUserCommand, ResponseBase<User>>
    {
        private readonly FinanceControlContex _context = contex;
        private readonly IConvertFormFileToBytes _convert = convert;    
        public async Task<ResponseBase<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == Guid.Empty)            
                return ResponseBase<User>.Fail("Id inválido", "Informe um id válido", 400);         
            
            var user = await _context.Users.Where(x => x.Id.Equals(request.Id)).FirstOrDefaultAsync();

            if (user == null)            
                return ResponseBase<User>.Fail("Usuário não encontrado", "Nenhum usuário com esse ID", 404);            

            user.Name = request.Name ?? user.Name;
            user.Surname = request.Surname ?? user.Surname;
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Username ?? user.UserName;
            user.Active = request.Active;
            user.AppRoleId = request.RoleId;

            if (request.Photo != null)
            {                
                user.PhotosUsers.PhotoUser = await _convert.ConvertToBytes(request.Photo);                
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ResponseBase<User>.Success(user);
        }
    }
}

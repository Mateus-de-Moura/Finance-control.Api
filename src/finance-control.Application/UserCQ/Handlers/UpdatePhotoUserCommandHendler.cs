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
    public class UpdatePhotoUserCommandHendler(FinanceControlContex contex, IConvertFormFileToBytes conversion) : IRequestHandler<UpdatePhotoUserCommand, ResponseBase<User>>
    {
        private readonly FinanceControlContex _context = contex;
        private readonly IConvertFormFileToBytes _conversion = conversion;
        public async Task<ResponseBase<User>> Handle(UpdatePhotoUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(x => x.PhotosUsers)
                .Where(x => x.Email.Equals(request.EmailUser!))
                .FirstOrDefaultAsync();

            if (user == null)
                return new ResponseBase<User>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Usuário inxistente",
                        ErrorDescription = "Nenhum usuário localizado com o e-mail informado.",
                        HttpStatus = 404
                    },
                    Value = null,
                };


            if (user.PhotosUsers == null)
                await _context.PhotosUsers.AddAsync(new PhotosUsers
                {
                    UserId = user.Id,
                    PhotoUser = await _conversion.ConvertToBytes(request.Photo)
                });
            else
                user.PhotosUsers.PhotoUser = await _conversion.ConvertToBytes(request.Photo);

            var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

            return rowsAffected > 0 ?
            new ResponseBase<User>
            {
                ResponseInfo = null,
                Value = user,
            } :
            new ResponseBase<User>
            {
                ResponseInfo = new ResponseInfo
                {
                    Title = "Erro ao atualizar",
                    ErrorDescription = "Não foi possivel alterar a foto do  usuário, tente novamente",
                    HttpStatus = 404
                },
                Value = null,
            };


        }
    }
}

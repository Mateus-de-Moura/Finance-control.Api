using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Handlers
{
    public class RecoveryPasswordHandler(FinanceControlContex contex) : IRequestHandler<RecoveryPasswordCommand, ResponseBase<bool>>
    {
        private readonly FinanceControlContex _contex = contex;
        public async Task<ResponseBase<bool>> Handle(RecoveryPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<bool>.Fail("Erro", "Guid invalido ou vazio", 400);

            var user = await  _contex.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.UserId));

            if(user == null)
                return ResponseBase<bool>.Fail("Erro", "Nenhum usuário localizado  com  o Id informado", 404);

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is true)
            {
                var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                var passWordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, passwordSalt);

                user.PasswordHash = passWordHash;
                user.PasswordSalt = passwordSalt;

                var rowsAffected = await _contex.SaveChangesAsync(cancellationToken);

                return rowsAffected > 0 ?
                    ResponseBase<bool>.Success(true) :
                    ResponseBase<bool>.Fail("Erro", "Ocorreu um erro ao atualizar a senha do usuário, tente novamente", 400);                
            }

            return ResponseBase<bool>.Fail("Erro", "A senha informada não é a mesma do cadastro, tente novamente", 400);
        }
    }
}

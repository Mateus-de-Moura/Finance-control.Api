using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class RecoveryPasswordHandler(IUserRepository userRepository) : IRequestHandler<RecoveryPasswordCommand, ResponseBase<bool>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<ResponseBase<bool>> Handle(RecoveryPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<bool>.Fail("Erro", "Guid invalido ou vazio", 400);

            var result = await _userRepository.GetById(request.UserId);

            if (!result.IsSuccess)
                return ResponseBase<bool>.Fail("Erro", "Nenhum usuário localizado  com  o Id informado", 404);

            var user = result.Value;

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is true)
            {
                var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                var passWordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, passwordSalt);

                user.PasswordHash = passWordHash;
                user.PasswordSalt = passwordSalt;

                var saved = await _userRepository.SaveChanges(cancellationToken);

                return saved ?
                    ResponseBase<bool>.Success(true) :
                    ResponseBase<bool>.Fail("Erro", "Ocorreu um erro ao atualizar a senha do usuário, tente novamente", 400);
            }

            return ResponseBase<bool>.Fail("Erro", "A senha informada não é a mesma do cadastro, tente novamente", 400);
        }
    }
}

using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class UpdatePhotoUserCommandHandler(IUserRepository userRepository, IConvertFormFileToBytes conversion) : IRequestHandler<UpdatePhotoUserCommand, ResponseBase<User>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConvertFormFileToBytes _conversion = conversion;
        public async Task<ResponseBase<User>> Handle(UpdatePhotoUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByEmail(request.EmailUser!);

            if (!result.IsSuccess)
                return ResponseBase<User>.Fail("Usuário inxistente", "Nenhum usuário localizado com o e-mail informado.", 404);

            var user = result.Value;

            if (user.PhotosUsers == null)
                user.PhotosUsers = new PhotosUsers
                {
                    UserId = user.Id,
                    PhotoUser = await _conversion.ConvertToBytes(request.Photo)
                };
            else
                user.PhotosUsers.PhotoUser = await _conversion.ConvertToBytes(request.Photo);

            var saved = await _userRepository.SaveChanges(cancellationToken);

            return saved ?
             ResponseBase<User>.Success(user) :
             ResponseBase<User>.Fail("Erro ao atualizar", "Não foi possivel alterar a foto do  usuário, tente novamente", 404);
        }
    }
}

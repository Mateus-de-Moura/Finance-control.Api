using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Interfaces.Repositories;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public record LoginUserCommandHandler(IUserRepository UserRepository, IAuthService AuthService,
        IMapper Mapper) : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IUserRepository _userRepository = UserRepository;
        private readonly IAuthService _authService = AuthService;
        private readonly IMapper _mapper = Mapper;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetForLogin(request.Email, matchEmail: request.Password == "user-github");

            if (!result.IsSuccess)
                return ResponseBase<RefreshTokenViewModel>.Fail("Usuário não encontrado", "Nenhum usuário encontrado com o nome de usuário informado.", 404);

            var user = result.Value;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return ResponseBase<RefreshTokenViewModel>.Fail("Dados incorretos", "Nome de usuário ou senha incorretos.", 404);

            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(7);

            await _userRepository.SaveChanges(cancellationToken);

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.UserName!, user.Id);

            if (user.PhotosUsers is not null)
                refreshTokenVM.Photo = Convert.ToBase64String(user.PhotosUsers.PhotoUser);

            return ResponseBase<RefreshTokenViewModel>.Success(refreshTokenVM);
        }
    }
}

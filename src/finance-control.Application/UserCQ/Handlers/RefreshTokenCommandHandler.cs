using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Interfaces.Repositories;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class RefreshTokenCommandHandler(IUserRepository userRepository, IMapper mapper,
        IAuthService authService) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return ResponseBase<RefreshTokenViewModel>.Fail("Token inválido", "RefreshToken inválido ou  expirado. Faça login novamente", 400);

            var result = await _userRepository.GetByRefreshToken(request.RefreshToken);
            var user = result.Value;

            if (!result.IsSuccess || user.RefreshTokenExpirationTime < DateTime.Now)
            {
                return ResponseBase<RefreshTokenViewModel>.Fail("Token inválido", "RefreshToken inválido ou  expirado. Faça login novamente", 400);
            }

            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(7);

            await _userRepository.SaveChanges(cancellationToken);

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.UserName!, user.Id);

            return  ResponseBase<RefreshTokenViewModel>.Success(refreshTokenVM);
        }
    }
}

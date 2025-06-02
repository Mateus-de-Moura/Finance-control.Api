using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Infra.Data;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Handlers
{
    public class RefreshTokenCommandHandler(FinanceControlContex context, IMapper mapper,
        IAuthService authService) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

            if (user == null || user.RefreshToken != request.RefreshToken
                || user.RefreshTokenExpirationTime < DateTime.Now)
            {
                return ResponseBase<RefreshTokenViewModel>.Fail("Token inválido", "RefreshToken inválido ou  expirado. Faça login novamente", 400);              
            }

            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync();

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.RefreshToken = _authService.GenerateJWT(user.Email!, user.UserName!, user.Id);

            return  ResponseBase<RefreshTokenViewModel>.Success(refreshTokenVM);           
        }
    }
}

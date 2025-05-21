using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Infra.Data;
using AutoMapper;
using finance_control.Application.UserCQ.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.UserCQ.Handlers
{
    public record LoginUserCommandHendler(FinanceControlContex contex, IAuthService AuthService,
        IMapper Mapper) : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly FinanceControlContex _contex = contex;
        private readonly IAuthService _authService = AuthService;
        private readonly IMapper _mapper = Mapper;
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _contex.Users
                .Include(x => x.Role)
                .Include(x => x.PhotosUsers)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
                return ResponseBase<RefreshTokenViewModel>.Fail("Usuário não encontrado", "Nenhum usuário encontrado com o email informado.", 404);

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) is true)
            {
                user.RefreshToken = _authService.GenerateRefreshToken();
                user.RefreshTokenExpirationTime = DateTime.Now.AddDays(7);

                _contex.Update(user);
                await _contex.SaveChangesAsync();

                RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
                refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.UserName!);

                if (user.PhotosUsers is not null)
                    refreshTokenVM.Photo = Convert.ToBase64String(user.PhotosUsers.PhotoUser);

                return ResponseBase<RefreshTokenViewModel>.Success(refreshTokenVM);
            }
            else
                return ResponseBase<RefreshTokenViewModel>.Fail("Senha incorreta", "A senha informada está incorreta.", 404);
        }
    }
}

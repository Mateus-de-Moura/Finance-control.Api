using AutoMapper;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class CreateUserCommandHendler(FinanceControlContex context, IMapper mapper,
        IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel?>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var isUniqueEmailAndUsername = await _authService.UniqueEmailAndUserName(request.Email!, request.Username!);

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.EmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Email Indisponivel",
                        ErrorDescription = "O Email Já está sendo utilizado. Tente outro.",
                        HttpStatus = 400
                    },
                    Value = null
                };
            }

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Username Indisponivel",
                        ErrorDescription = "O username Já está sendo utilizado. Tente outro.",
                        HttpStatus = 400
                    },
                    Value = null
                };
            }
            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameAndEmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Username e Email Indisponíveis",
                        ErrorDescription = "o username e o email apresentados já estão sendo utilizados. Tente outro.",
                        HttpStatus = 400
                    },
                    Value = null
                };
            }

            var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var passWordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, passwordSalt);

            var user = _mapper.Map<User>(request);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passWordHash;
            user.AppRoleId = request.RoleId;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.UserName!);

            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = null,
                Value = refreshTokenVM,
            };
        }
    }
}

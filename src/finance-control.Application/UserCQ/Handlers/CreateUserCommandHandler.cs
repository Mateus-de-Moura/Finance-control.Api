using AutoMapper;
using finance_control.Application.UserCQ.Queries;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.UserCQ.Handlers
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper,
        IAuthService authService,  IConvertFormFileToBytes convert) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel?>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;
        private readonly IConvertFormFileToBytes _convert = convert;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var isUniqueEmailAndUsername = await _authService.UniqueEmailAndUserName(request.Email!, request.Username!);

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.EmailUnavailable)
                return ResponseBase<RefreshTokenViewModel>.Fail("Email Indisponivel", "O Email Já está sendo utilizado. Tente outro.", 400);


            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameUnavailable)
                return ResponseBase<RefreshTokenViewModel>.Fail("Username Indisponivel", "O username Já está sendo utilizado. Tente outro.", 400);

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameAndEmailUnavailable)
                return ResponseBase<RefreshTokenViewModel>.Fail("Username e Email Indisponíveis",
                    "o username e o email apresentados já estão sendo utilizados. Tente outro.", 400);

            var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var passWordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, passwordSalt);

            var user = _mapper.Map<User>(request);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passWordHash;
            user.AppRoleId = request.RoleId;

            if (request.Photo is not null)
                user.PhotosUsers = new PhotosUsers
                {
                    PhotoUser = await _convert.ConvertToBytes(request.Photo!)
                };

            var created = await _userRepository.Create(user);

            if (!created.IsSuccess)
                return ResponseBase<RefreshTokenViewModel>.Fail("Erro ao cadastrar", "Não foi possível cadastrar o usuário, tente novamente.", 400);

            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.UserName!, user.Id);

            return  ResponseBase<RefreshTokenViewModel>.Success(refreshTokenVM);          
        }     
    }
}

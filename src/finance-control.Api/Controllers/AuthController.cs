using AutoMapper;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Queries;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace finance_control.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IMediator madiator, IMapper mapper, ILogger<AuthController> logger) : ControllerBase
    {
        private const string AuthTokenCookie = "AuthToken";
        private const string RefreshTokenCookie = "RefreshToken";

        private readonly IMediator _mediator = madiator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("Login")]
        public async Task<ActionResult<UserInfoViewModel>> Login(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ResponseInfo is null && request.Value is not null)
            {
                AppendAuthCookies(request.Value);

                _logger.LogInformation("Operação de login do usuário {Name} foi realizada com sucesso", command.Email);
                return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
            }

            return BadRequest(request);
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<UserInfoViewModel>> RefreshToken()
        {
            var refreshToken = Request.Cookies[RefreshTokenCookie];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "Refresh token ausente" });

            var response = await _mediator.Send(new RefreshTokenCommand { RefreshToken = refreshToken });

            if (response.ResponseInfo is not null || response.Value is null)
            {
                DeleteAuthCookies();
                return Unauthorized(response.ResponseInfo);
            }

            AppendAuthCookies(response.Value);

            _logger.LogInformation("Cookies de autenticação renovados para usuário {Username}", response.Value.Username);
            return Ok(_mapper.Map<UserInfoViewModel>(response.Value));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var username = HttpContext.User.Identity?.Name ?? "Unknown";

            DeleteAuthCookies();

            _logger.LogInformation("Logout realizado com sucesso - cookies de autenticação removidos para usuário: {Username}", username);
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var username = HttpContext.User.Claims.First();

                var response = await _mediator.Send(new GetUserAuthQuery { UserNameOrEmailAddress = username.Value });

                var user = response.ResponseInfo is null ? _mapper.Map<UserInfoViewModel>(response.Value) : null;

                return Ok(new { authenticated = true, user = user });
            }

            _logger.LogWarning("Usuário não autenticado - retornando 401.");
            return Unauthorized();
        }

        [HttpPost("LoginGithub")]
        public async Task<ActionResult<UserInfoViewModel>> LoginGithub([FromBody] LoginGithubCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseInfo != null || response.Value is null)
                return BadRequest(response);

            AppendAuthCookies(response.Value);

            return Ok(_mapper.Map<UserInfoViewModel>(response.Value));
        }

        private CookieOptions BuildCookieOptions(DateTimeOffset expires) => new()
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = expires,
            Path = "/"
        };

        private void AppendAuthCookies(RefreshTokenViewModel tokens)
        {
            Response.Cookies.Append(AuthTokenCookie, tokens.TokenJwt!, BuildCookieOptions(DateTimeOffset.UtcNow.AddHours(1)));

            var refreshExpires = tokens.RefreshTokenExpirationTime is DateTime expiration
                ? new DateTimeOffset(expiration)
                : DateTimeOffset.UtcNow.AddDays(7);

            Response.Cookies.Append(RefreshTokenCookie, tokens.RefreshToken!, BuildCookieOptions(refreshExpires));
        }

        private void DeleteAuthCookies()
        {
            var expired = BuildCookieOptions(DateTimeOffset.UtcNow.AddDays(-1));

            Response.Cookies.Delete(AuthTokenCookie, expired);
            Response.Cookies.Delete(RefreshTokenCookie, expired);
        }
    }
}

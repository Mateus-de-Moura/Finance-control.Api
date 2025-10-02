using System.Net;
using System.Net.Mail;
using AutoMapper;
using Azure;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Query;
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
        private readonly IMediator _mediator = madiator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> Login(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ResponseInfo is null)
            {
                var userInfo = request.Value;

                if (userInfo is not null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1),
                        Path = "/"
                    };

                    Response.Cookies.Append("AuthToken", request.Value.TokenJwt!, cookieOptions);                      
                    request.Value.TokenJwt = string.Empty;

                    _logger.LogInformation("Operação de login do usuário {Name} foi realizada com sucesso", command.Email);
                    return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
                }
            }

            return BadRequest(request);
        }

        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> RefreshToken(RefreshTokenCommand comand)
        {
            if (string.IsNullOrEmpty(comand.Username) || string.IsNullOrEmpty(comand.RefreshToken))
                return BadRequest(new { message = "Usuário ou token inválidos" });

            var response = await _mediator.Send(new RefreshTokenCommand
            {
                Username = comand.Username,
                RefreshToken = comand.RefreshToken
            });

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(1),
                Path = "/"
            };

            Response.Cookies.Append("AuthToken", response.Value.TokenJwt!, cookieOptions);            
        
            _logger.LogInformation("Cookie AuthToken renovado para usuário {Username}", comand.Username);
            response.Value.TokenJwt = string.Empty;

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {            
            var username = HttpContext.User.Identity?.Name ?? "Unknown";
            _logger.LogInformation("Iniciando logout para usuário: {Username}", username);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/" 
            };
            
            Response.Cookies.Append("AuthToken", "", cookieOptions);
       
            Response.Cookies.Delete("AuthToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            _logger.LogInformation("Logout realizado com sucesso - cookie AuthToken removido para usuário: {Username}", username);
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {           
            var hasCookie = Request.Cookies.ContainsKey("AuthToken");
            var cookieValue = Request.Cookies["AuthToken"];
            
            _logger.LogInformation("Verificando autenticação - Cookie presente: {HasCookie}, Valor: {CookieValue}", 
                hasCookie, string.IsNullOrEmpty(cookieValue) ? "VAZIO" : "PRESENTE");

            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var username = HttpContext.User.Claims.First();
                _logger.LogInformation("Usuário autenticado: {Username}", username.Value);

                var response = await _mediator.Send(new GetUserAuthQuery { UserNameOrEmailAddress = username.Value });

                var user = response.ResponseInfo is null ? response.Value : null;

                return Ok(new { authenticated = true, user = user });
            }

            _logger.LogWarning("Usuário não autenticado - retornando 401. Cookie presente: {HasCookie}", hasCookie);
            return Unauthorized();
        }

        [HttpPost("LoginGithub")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> LoginGithub([FromBody] LoginGithubCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseInfo != null)
                return BadRequest(response);

            return Ok(response);
        }

    }
}

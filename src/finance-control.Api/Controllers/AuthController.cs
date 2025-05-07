using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace finance_control.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IMediator madiator, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = madiator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> Login(LoginUserCommand command)
        {
            var request = await _mediator.Send(command);

            if (request.ResponseInfo is null)
            {
                var userInfo = request.Value;

                if (userInfo is not null)
                {                 
                    return Ok(_mapper.Map<UserInfoViewModel>(request.Value));
                }
            }

            return BadRequest(request);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> RefreshToken(RefreshTokenCommand comand)
        {
            var request = await _mediator.Send(new RefreshTokenCommand
            {
                Username = comand.Username,
                RefreshToken = comand.RefreshToken
            });

            return Ok(request);
        }     
    }
}

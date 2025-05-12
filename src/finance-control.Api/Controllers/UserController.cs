using AutoMapper;
using Azure.Core;
using finance_control.Application.Common.Models;
using finance_control.Application.UserCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   
    public class UserController(IMediator madiator, IMemoryCache cache) : ControllerBase
    {
        private readonly IMediator _mediator = madiator;
        private readonly IMemoryCache _cache = cache;

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedRequest request)
        {
            var response = await _mediator.Send(new GetPagedUsersCommand
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Name = request.Description,
            });


            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand request)
        {
            var response = await _mediator.Send(request);

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _mediator.Send(new GetRolesToUserCommand());

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }
    }
}

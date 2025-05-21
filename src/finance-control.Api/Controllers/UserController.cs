using AutoMapper;
using Azure.Core;
using finance_control.Application.Common.Models;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.Queries;
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
        public async Task<IActionResult> Create([FromForm] CreateUserCommand request)
        {
            var response = await _mediator.Send(request);

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(Guid Id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { Id = Id });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            const string cacheKey = "Roles";

            var response = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var responseDb = await _mediator.Send(new GetRolesToUserCommand());
                return responseDb;
            });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserCommand user)
        {
            var response = await _mediator.Send(user);

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpPut("update-photo")]
        public async Task<IActionResult> UpdatePhotoUser([FromForm] UpdatePhotoUserCommand command)
        {
            var response = await _mediator.Send(new UpdatePhotoUserCommand { Photo = command.Photo, EmailUser = command.EmailUser });

            if (response.ResponseInfo is null)
                return Ok(Convert.ToBase64String(response.Value.PhotosUsers.PhotoUser));

            return BadRequest(response.ResponseInfo);

        }
    }
}

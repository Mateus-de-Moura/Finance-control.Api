using Azure;
using finance_control.Api.Interfaces;
using finance_control.Application.LocationDataCQ.Command;
using finance_control.Application.LocationDataCQ.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationDataController(IMediator mediator, IUserContext userContext ) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserContext _userContext = userContext;

        [HttpPost]
        public async Task<IActionResult> AddLocation(AddLocationDataCommand command)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            command.Ip = ipAddress;

            var response = await _mediator.Send(command);

            if (response.ResponseInfo is null)
                return Created(string.Empty, response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocation()
        {
            var response = await _mediator.Send(new GetLocationDataByUserQuery { UserId = _userContext.UserId});

            if (response.ResponseInfo is null)
                return Created(string.Empty, response.Value);

            return BadRequest(response.ResponseInfo);
        }
    }
}

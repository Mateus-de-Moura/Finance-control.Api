using Azure;
using finance_control.Application.LocationDataCQ.Command;
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
    public class LocationDataController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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
    }
}

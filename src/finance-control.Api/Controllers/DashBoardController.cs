using finance_control.Application.DashBoardCQ.Query;
using finance_control.Application.RevenuesCQ.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDashBoardByUserId(Guid userId)
        {
            var response = await _mediator.Send(new DashBoardQuery { UserId = userId });

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

    }
}

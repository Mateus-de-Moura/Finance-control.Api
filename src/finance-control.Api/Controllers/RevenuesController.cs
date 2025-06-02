using System.Security.Claims;
using finance_control.Application.Common.Models;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Application.RevenuesCQ.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController(IMediator madiator) : ControllerBase
    {
        private readonly IMediator _mediator = madiator;     

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedRequest request)
        {
            var response = await _mediator.Send(new GetPagedRevenuesQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Description = request.Description,
            });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRevenueCommand command)
        {
            var userId = User.FindFirst("UserId")?.Value;
            command.UserId = Guid.Parse(userId);

            var response = await _mediator.Send(command);

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(Guid Id)
        {
            var response = await _mediator.Send(new GetRevenuesByIdQuery { Id = Id});

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateRevenues([FromBody] UpdateRevenueCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);

        }
    }
}

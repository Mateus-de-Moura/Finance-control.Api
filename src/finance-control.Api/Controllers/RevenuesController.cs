using finance_control.Application.Common.Models;
using finance_control.Application.RevenuesCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            var response = await _mediator.Send(new GetPagedRevenuesCommand
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
            var response = await _mediator.Send(command);

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

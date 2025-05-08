using AutoMapper;
using Azure;
using finance_control.Application.Common.Models;
using finance_control.Application.RevenuesCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController(IMediator madiator, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = madiator;
        private readonly IMapper _mapper = mapper;

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
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create ([FromQuery] CreateRevenueCommand request)
        {
           var  response = await _mediator.Send(request);

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }
    }
}

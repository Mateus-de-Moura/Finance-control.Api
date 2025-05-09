using AutoMapper;
using Azure;
using finance_control.Application.Common.Models;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController(IMediator madiator, IMapper mapper, IMemoryCache cache) : ControllerBase
    {
        private readonly IMediator _mediator = madiator;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _cache = cache;

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedRequest request)
        {
            const string cacheKey = "Revenues";

            var response = await _cache.GetOrCreate(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(1);
                entry.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(5);

                var responseDb = await _mediator.Send(new GetPagedRevenuesCommand
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Description = request.Description,
                });

                return responseDb;
            });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] CreateRevenueCommand request)
        {
            var response = await _mediator.Send(request);

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }
    }
}

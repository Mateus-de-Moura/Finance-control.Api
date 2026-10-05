using Azure;
using finance_control.Api.Interfaces;
using finance_control.Application.CategoryCQ.Commands;
using finance_control.Application.CategoryCQ.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(IMemoryCache cache, IMediator mediator, IUserContext userContext) : ControllerBase
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IMediator _mediator = mediator;
        private readonly IUserContext _userContext = userContext;

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            const string cacheKey = "category";

            var response = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                var responseDb = await _mediator.Send(new GetAllCategoryQuery());
                return responseDb;
            });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedCategory([FromQuery] GetPagedCategoryQuery request)
        {
            var result = await _mediator.Send(request);

            if (result.ResponseInfo is null)
                return Ok(result.Value);

            return BadRequest(result.ResponseInfo);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            var response = await _mediator.Send(new GetByIdCategoryQuery { Id = id });

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            command.UserId = _userContext.UserId;

            var result = await _mediator.Send(command);
            _cache.Remove("category");

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ResponseInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            _cache.Remove("category");

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ResponseInfo);
        }
    }
}

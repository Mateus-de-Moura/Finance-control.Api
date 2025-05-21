using finance_control.Application.CategoryCQ.Queries;
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
    public class CategoryController(IMemoryCache cache, IMediator mediator) : ControllerBase
    {
        private readonly IMemoryCache _cache = cache;
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            const string cacheKey = "category";

            var response = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var responseDb = await _mediator.Send(new GetAllCategoryQuery());
                return responseDb;
            });

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }
    }
}

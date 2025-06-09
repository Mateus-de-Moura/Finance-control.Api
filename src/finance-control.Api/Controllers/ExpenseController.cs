using finance_control.Api.Interfaces;
using finance_control.Application.Common.Models;
using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.ExpenseCQ.Query;
using finance_control.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace finance_control.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;

        public ExpenseController(IMediator mediator, IUserContext userContext)
        {
            _mediator = mediator;
            _userContext = userContext;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedRequestExpense request)
        {
            var result = await _mediator.Send(new GetPagedExpenseQuery
            {
                UserId = _userContext.UserId,
                CategoryId = request.CategoryId,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                Status = request.Status,
                Description = request.Description,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            });

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ResponseInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command)
        {
            command.UserId = _userContext.UserId;

            var result = await _mediator.Send(command);

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ResponseInfo);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UpdateExpenseCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ResponseInfo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteExpenseCommand(id));

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return NoContent();
        }

        [HttpPut("{id}/Despesas")]
        public async Task<IActionResult> Payment(Guid id, [FromBody] PaymentExpenseCommand command )
        {
            var result = await _mediator.Send(command);

            if (result.ResponseInfo is null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.ResponseInfo);

        }
    }
}

using Azure;
using Azure.Core;
using finance_control.Api.Interfaces;
using finance_control.Application.TransactionsCQ.Command;
using finance_control.Application.TransactionsCQ.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace finance_control.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController(IMediator mediator, IUserContext userContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IUserContext _userContext = userContext;

        [HttpPost]
        public async Task<IActionResult> AddTransaction(AddTransactionsCommand transaction)
        {
            transaction.UserId = _userContext.UserId;
            if (transaction == null)
                return BadRequest();

            var response = await _mediator.Send(transaction);

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedTransactionsQuey request)
        {
            request.UserId = _userContext.UserId;
            var response = await _mediator.Send(request);

            if (response.ResponseInfo is null)
                return Ok(response.Value);

            return BadRequest(response.ResponseInfo);
        }
    }
}

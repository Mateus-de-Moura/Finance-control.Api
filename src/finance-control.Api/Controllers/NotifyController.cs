﻿using finance_control.Application.NotifyCQ.Command;
using finance_control.Application.NotifyCQ.Query;
using finance_control.Domain.Entity;
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
    public class NotifyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetNotifyByUserId(string wasRead)
        {
            var UserId = Guid.Parse(User.FindFirst("UserId")?.Value);

            var response = await _mediator.Send(new NotifyQuery { UserId = UserId, wasRead = wasRead });

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

        [HttpGet("GetNotificationDoesNotRead")]
        public async Task<IActionResult> GetNotificationDoesNotRead()
        {
            var UserId = Guid.Parse(User.FindFirst("UserId")?.Value);

            var response = await _mediator.Send(new GetQuantityNotifyByUser { UserId = UserId });

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateNotificationById(Guid Id)
        {
            var response = await _mediator.Send(new UpdateNotifyCommand { Id = Id });

            if (response.ResponseInfo is null)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ResponseInfo);
        }
    }
}

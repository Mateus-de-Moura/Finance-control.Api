﻿using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.UserCQ.Commands
{
    public record CreateUserCommand : IRequest<ResponseBase<RefreshTokenViewModel?>>
    {
        public bool Active { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public Guid RoleId { get; set; }

        public IFormFile Photo { get; set; }
    }
}

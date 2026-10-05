using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using MediatR;

namespace finance_control.Application.UserCQ.Commands
{
    public record RefreshTokenCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? RefreshToken { get; set; }
    }
}

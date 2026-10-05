using finance_control.Application.Response;
using finance_control.Application.UserCQ.Queries;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class GetRolesToUserCommandHandler(IUserRepository userRepository) : IRequestHandler<GetRolesToUserCommand, ResponseBase<List<AppRole>>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<ResponseBase<List<AppRole>>> Handle(GetRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var roles = await _userRepository.GetRoles();

            return  ResponseBase<List<AppRole>>.Success(roles);
        }
    }
}

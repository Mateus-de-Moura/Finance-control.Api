using finance_control.Application.Response;
using finance_control.Application.UserCQ.Queries;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.UserCQ.Handlers
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, ResponseBase<User>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<ResponseBase<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<User>.Fail("Id inválido", "Informe um Guid válido", 404);

            var result = await _userRepository.GetById(request.Id);

            return result.IsSuccess ?
                ResponseBase<User>.Success(result.Value) :
                ResponseBase<User>.Fail("Id inválido", "Informe um Guid válido", 404);
        }
    }
}

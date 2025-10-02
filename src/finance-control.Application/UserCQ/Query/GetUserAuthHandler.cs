using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.UserCQ.Query
{
    public class GetUserAuthHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserAuthQuery, ResponseBase<RefreshTokenViewModel>>
    {
        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(GetUserAuthQuery request, CancellationToken cancellationToken)
        {
            var result = await userRepository.GetUserByEmailOrName(request.UserNameOrEmailAddress);

            var userMapped = mapper.Map<RefreshTokenViewModel>(result.Value);

            if (result.Value.PhotosUsers is not null)
                userMapped.Photo = Convert.ToBase64String(result.Value.PhotosUsers.PhotoUser);

            return result.IsSuccess ?
                ResponseBase<RefreshTokenViewModel>.Success(userMapped) :
                ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Usuario nao foi localizado", 404);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Handlers
{
    public class UpdateUserCommandHendler(IMapper mapper, IConvertFormFileToBytes convert,
        IUserRepository UserRepository) : IRequestHandler<UpdateUserCommand, ResponseBase<User>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IConvertFormFileToBytes _convert = convert;
        private readonly IUserRepository _userRepository = UserRepository;
        public async Task<ResponseBase<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<User>.Fail("Id inválido", "Informe um id válido", 400);

            var user = _mapper.Map<User>(request);

            if (user == null)
               return ResponseBase<User>.Fail("Erro", "Erro ao atualizar usuário", 400);

            if (request.Photo != null)
                user.PhotosUsers.PhotoUser = await _convert.ConvertToBytes(request.Photo);

            var result = await _userRepository.Update(user);

            return result.IsSuccess ?
                ResponseBase<User>.Success(user) :
                ResponseBase<User>.Fail("Erro", "Erro ao atualizar usuário", 400);
        }
    }
}

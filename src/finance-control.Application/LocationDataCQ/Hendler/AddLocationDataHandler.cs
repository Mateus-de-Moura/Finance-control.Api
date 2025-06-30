using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.LocationDataCQ.Command;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.LocationDataCQ.Hendler
{
    public class AddLocationDataHandler(FinanceControlContex context, ILoginLocationDataRepository loginLocationData) : IRequestHandler<AddLocationDataCommand, ResponseBase<LoginLocationData>>
    {
        private readonly FinanceControlContex _contex = context;
        private readonly ILoginLocationDataRepository _loginLocationDataRepository = loginLocationData;
        public async Task<ResponseBase<LoginLocationData>> Handle(AddLocationDataCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
                return ResponseBase<LoginLocationData>.Fail("E-mail inválido", "Email vazio,  informe um e-mail válido", 400);

            var user = await _contex.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

            if (user == null)
                return ResponseBase<LoginLocationData>.Fail("Usuário inexistente", "Não foi encontrado nenhum usuário com o e-mail informado", 404);

            var loginLocationData = new LoginLocationData
            {
                IsSuccess = request.IsSuccess,
                AccessDate = DateTime.Now,
                EmailRequest = request.Email,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Ip = request.Ip,
                Platform = request.Platform,
                Browser = request.Browser,
                Os = request.Os,
                UserId = user.Id,
            };

            await _contex.LoginLocationData.AddAsync(loginLocationData);

            var result = await _loginLocationDataRepository.Create(loginLocationData);

            return result.IsSuccess  ?
                ResponseBase<LoginLocationData>.Success(loginLocationData) :
                ResponseBase<LoginLocationData>.Fail("Erro ao salvar", "Tente novamente", 400);
        }
    }
}

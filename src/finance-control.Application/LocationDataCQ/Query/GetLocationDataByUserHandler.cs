using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.LocationDataCQ.Query
{
    public class GetLocationDataByUserHandler(ILoginLocationDataRepository loginLocationDataRepository) : IRequestHandler<GetLocationDataByUserQuery, ResponseBase<List<LoginLocationData>>>
    {        
        private readonly ILoginLocationDataRepository _loginLocationDataRepository = loginLocationDataRepository;
        public async Task<ResponseBase<List<LoginLocationData>>> Handle(GetLocationDataByUserQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<List<LoginLocationData>>.Fail("Guid inválido", "Informe um id válido", 400);

            var result = await _loginLocationDataRepository.GetLocations(request.UserId);

            return result.IsSuccess ?
                ResponseBase<List<LoginLocationData>>.Success(result) :
                ResponseBase<List<LoginLocationData>>.Fail("Not Found", "nenhum item localizado", 404);
        }
    }
}

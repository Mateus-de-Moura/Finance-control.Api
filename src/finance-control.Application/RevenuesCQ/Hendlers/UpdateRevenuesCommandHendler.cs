using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Hendlers
{
    public class UpdateRevenuesCommandHendler(IRevenuesRepository revenuesRepository ) : IRequestHandler<UpdateRevenueCommand , ResponseBase<Revenues>>
    {              
        public async Task<ResponseBase<Revenues>> Handle(UpdateRevenueCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                ResponseBase<Revenues>.Fail("Erro ao atualizar", "Preencha todos os campos e tente novamente", 400);

            var result = await revenuesRepository.UpdateRevenue(new Revenues
            {
                Id = request.Id,
                Active = request.Active,
                IsRecurrent = request.Recurrent,
                Description = request.Description,
                Value = request.Value,
                Date = request.Date,
                CategoryId = request.CategoryId,
            });

            return result.IsSuccess ?
                ResponseBase<Revenues>.Success(result.Value) :
                ResponseBase<Revenues>.Fail("Erro ao atualizar", "tente novamente", 400);       
        }
    }
}

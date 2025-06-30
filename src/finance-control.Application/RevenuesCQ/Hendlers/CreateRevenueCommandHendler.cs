using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;

namespace finance_control.Application.RevenuesCQ.Hendlers
{
    public class CreateRevenueCommandHendler(IRevenuesRepository revenuesRepository) : IRequestHandler<CreateRevenueCommand, ResponseBase<Revenues?>>
    {
        private readonly IRevenuesRepository _revenues = revenuesRepository;
        public async Task<ResponseBase<Revenues>> Handle(CreateRevenueCommand request, CancellationToken cancellationToken)
        {
            var result = await _revenues.CreateRevenue(new Revenues
            {
                Active = true,
                Description = request.Description,
                Value = request.Value,
                Date = request.Date,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
            });

            return result.IsSuccess ?
                 ResponseBase<Revenues>.Success(result.Value) :
                 ResponseBase<Revenues>.Fail("Erro ao cadastrar Receita", "Ocorreu um erro ao salvar os dados, tente novamente.", 400);
        }
    }
}

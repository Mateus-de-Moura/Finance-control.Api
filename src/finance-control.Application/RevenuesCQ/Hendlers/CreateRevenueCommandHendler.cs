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
using finance_control.Infra.Data;
using MediatR;

namespace finance_control.Application.RevenuesCQ.Hendlers
{
    public class CreateRevenueCommandHendler(FinanceControlContex context) : IRequestHandler<CreateRevenueCommand, ResponseBase<Revenues?>>
    {
        private readonly FinanceControlContex _context = context;
        public async Task<ResponseBase<Revenues?>> Handle(CreateRevenueCommand request, CancellationToken cancellationToken)
        {
            var revenue = new Revenues
            {
                Active = true,
                Description = request.Description,
                Value = request.Value,
                Date = request.Date,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
            };

            await _context.AddAsync(revenue);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                new ResponseBase<Revenues?>
                {
                    ResponseInfo = null,
                    Value = revenue
                } :

                new ResponseBase<Revenues?>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Erro ao cadastrar Receita",
                        ErrorDescription = "Ocorreu um erro ao salvar os dados, tente novamente.",
                        HttpStatus = 404
                    },
                    Value = null
                };


        }
    }
}

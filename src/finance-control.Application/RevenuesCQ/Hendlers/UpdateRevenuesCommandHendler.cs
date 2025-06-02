using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.RevenuesCQ.Hendlers
{
    public class UpdateRevenuesCommandHendler(FinanceControlContex context) : IRequestHandler<UpdateRevenueCommand , ResponseBase<Revenues>>
    {     
        private readonly FinanceControlContex _context = context;

        public async Task<ResponseBase<Revenues>> Handle(UpdateRevenueCommand request, CancellationToken cancellationToken)
        {
            var revenue = await _context.Revenues.Where(x => x.Id.Equals(request.Id)).FirstOrDefaultAsync();

            if (revenue == null)
            {
                return new ResponseBase<Revenues>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Nenhum registro encontrado.",
                        ErrorDescription = "não foi possivel localizar um registro com o Id informado, tente novamente",
                        HttpStatus = 404
                    },
                    Value = null,
                };
            }

            revenue.Active = request.Active;
            revenue.IsRecurrent = request.Recurrent;
            revenue.Description = request.Description;
            revenue.Value = request.Value;
            revenue.Date = request.Date;
            revenue.CategoryId = request.CategoryId;

            _context.Entry(revenue).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new ResponseBase<Revenues>
            {
                ResponseInfo = null,
                Value = revenue,
            };

        }
    }
}

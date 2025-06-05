using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.NotifyCQ.Query
{
    public class NotifyHandler(FinanceControlContex contex) : IRequestHandler<NotifyQuery, ResponseBase<List<Notify>>>
    {
        private readonly FinanceControlContex _contex = contex;
        public async  Task<ResponseBase<List<Notify>>> Handle(NotifyQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<List<Notify>>.Fail("Guid Inválido", "Guid vazio ou inválido, tente novamente", 404);

            try
            {
                var notify = await _contex.Notify.Where(x => x.UserId.Equals(request.UserId))
                    .Include(x => x.Expenses)
                    .ToListAsync();

                return ResponseBase<List<Notify>>.Success(notify);
            }
            catch (Exception ex)
            {
                return ResponseBase<List<Notify>>.Fail("Erro", ex.Message, 404);
            }
        }
    }
}

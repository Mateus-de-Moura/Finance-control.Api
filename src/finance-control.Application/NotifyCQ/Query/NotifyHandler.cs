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
        public async Task<ResponseBase<List<Notify>>> Handle(NotifyQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<List<Notify>>.Fail("Guid Inválido", "Guid vazio ou inválido, tente novamente", 404);

            try
            {
                var queryable = contex.Notify
                    .Where(x => x.UserId.Equals(request.UserId))
                    .Include(x => x.Expenses)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.wasRead))
                {
                    if (request.wasRead == "read")
                        queryable = queryable.Where(x => x.WasRead == true);
                    else
                        queryable = queryable.Where(x => x.WasRead == false);
                }

                var notify = await queryable.ToListAsync();

                return ResponseBase<List<Notify>>.Success(notify);
            }
            catch (Exception ex)
            {
                return ResponseBase<List<Notify>>.Fail("Erro", ex.Message, 404);
            }
        }
    }
}

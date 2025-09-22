using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.NotifyCQ.Query
{
    public class GetQuantityNotifyByUserHandler(FinanceControlContex context) : IRequestHandler<GetQuantityNotifyByUser, ResponseBase<int>>
    {        
        public async Task<ResponseBase<int>> Handle(GetQuantityNotifyByUser request, CancellationToken cancellationToken)
        {
            var totalNotification = await context.Notify.Where(x => !x.WasRead).CountAsync();

            return totalNotification > 0 ?
                ResponseBase<int>.Success(totalNotification) :
                ResponseBase<int>.Fail("Erro", "Nenhuma entidade encontrada", 404);

        }
    }
}

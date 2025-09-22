using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.NotifyCQ.Command;
using finance_control.Application.Response;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.NotifyCQ.Handler
{
    public class UpdateNotifyHandler(INotificationRepository notification) : IRequestHandler<UpdateNotifyCommand, ResponseBase<bool>>
    {
        public async Task<ResponseBase<bool>> Handle(UpdateNotifyCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<bool>.Fail("Erro", "Informe um Id válido", 400);

            var response = await notification.UpdateNotification(request.Id);

            return response.IsSuccess ?
                ResponseBase<bool>.Success(response.Value) :
                ResponseBase<bool>.Fail("Erro ao atualizar", "Tente novamente", 400);

        }
    }
}

using finance_control.Application.NotifyCQ.Queries;
using finance_control.Application.Response;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.NotifyCQ.Handlers
{
    public class GetQuantityNotifyByUserHandler(INotificationRepository repository) : IRequestHandler<GetQuantityNotifyByUser, ResponseBase<int>>
    {
        public async Task<ResponseBase<int>> Handle(GetQuantityNotifyByUser request, CancellationToken cancellationToken)
        {
            var totalNotification = await repository.CountUnreadByUser(request.UserId);

            return totalNotification > 0 ?
                ResponseBase<int>.Success(totalNotification) :
                ResponseBase<int>.Fail("Erro", "Nenhuma entidade encontrada", 404);

        }
    }
}

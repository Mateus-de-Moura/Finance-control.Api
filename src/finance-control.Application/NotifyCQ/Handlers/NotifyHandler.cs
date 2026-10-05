using finance_control.Application.NotifyCQ.Queries;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.NotifyCQ.Handlers
{
    public class NotifyHandler(INotificationRepository repository) : IRequestHandler<NotifyQuery, ResponseBase<List<Notify>>>
    {
        public async Task<ResponseBase<List<Notify>>> Handle(NotifyQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return ResponseBase<List<Notify>>.Fail("Guid Inválido", "Guid vazio ou inválido, tente novamente", 404);

            try
            {
                bool? wasRead = string.IsNullOrEmpty(request.wasRead) ? null : request.wasRead == "read";

                var notify = await repository.GetByUser(request.UserId, wasRead);

                return ResponseBase<List<Notify>>.Success(notify);
            }
            catch (Exception ex)
            {
                return ResponseBase<List<Notify>>.Fail("Erro", ex.Message, 404);
            }
        }
    }
}

using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        public Task<Result<bool>> UpdateNotification(Guid Id);
        public Task<List<Notify>> GetByUser(Guid userId, bool? wasRead);
        public Task<int> CountUnreadByUser(Guid userId);
    }
}

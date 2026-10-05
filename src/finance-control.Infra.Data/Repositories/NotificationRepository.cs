using Ardalis.Result;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class NotificationRepository(FinanceControlContex contex) : INotificationRepository
    {
        private readonly FinanceControlContex _contex = contex;
        public async Task<Result<bool>> UpdateNotification(Guid Id)
        {
            var notifications = await _contex.Notify.Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync();

            if (notifications == null)
                return Result.Error("Entidade nao encontrada");

            var result = await _contex.Notify.Where(x => x.Id.Equals(Id))
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.WasRead, true)
                 .SetProperty(x => x.ReadDate, DateTime.Now));

            return result > 0 ?
                Result<bool>.Success(true) :
                Result.Error("");
        }

        public async Task<List<Notify>> GetByUser(Guid userId, bool? wasRead)
        {
            var queryable = _contex.Notify
                .Where(x => x.UserId.Equals(userId))
                .Include(x => x.Expenses)
                .AsQueryable();

            if (wasRead is not null)
                queryable = queryable.Where(x => x.WasRead == wasRead);

            return await queryable.ToListAsync();
        }

        public async Task<int> CountUnreadByUser(Guid userId)
        {
            return await _contex.Notify.CountAsync(x => x.UserId.Equals(userId) && !x.WasRead);
        }
    }
}

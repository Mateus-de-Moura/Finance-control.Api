using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
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
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.WasRead, true));

            return result > 0 ?
                Result<bool>.Success(true) :
                Result.Error("");
        }
    }
}

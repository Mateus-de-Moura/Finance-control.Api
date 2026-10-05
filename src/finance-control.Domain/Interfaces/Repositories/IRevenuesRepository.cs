using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface IRevenuesRepository
    {
        Task<Result<Revenues>> CreateRevenue(Revenues revenues);
        Task<Result<Revenues>> UpdateRevenue(Revenues revenues);
        Task<Result<Revenues>> GetRevenuesById(Guid RevenuesId);
        IQueryable<Revenues> GetRevenuesFilter();
    }
}

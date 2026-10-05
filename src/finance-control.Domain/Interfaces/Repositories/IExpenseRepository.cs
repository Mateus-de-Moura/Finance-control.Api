using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface IExpenseRepository
    {
        Task<Result<Expenses>> Create(Expenses expense);
        IQueryable<Expenses> GetExpensesFilter();
        Task<Result<Expenses>> GetById(Guid id);
        Task<Result<Expenses>> Update(Expenses expense);
        Task<Result<bool>> Delete(Guid id);
    }
}

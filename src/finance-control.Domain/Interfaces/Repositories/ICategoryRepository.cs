using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository 
    {
        public Task<List<Category>> GetAllCategory();

        public IQueryable<Category> GetCategoryFilter();

        public Task<Result<Category>> CreateCategory(Category category);

        public Task<Result<Category>> UpdateCategory(Category updateCategory);
    }
}

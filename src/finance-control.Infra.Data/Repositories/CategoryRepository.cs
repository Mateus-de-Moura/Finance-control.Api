using Ardalis.Result;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class CategoryRepository(FinanceControlContex context) : ICategoryRepository
    {
        private readonly FinanceControlContex _context = context;

        public async Task<List<Category>> GetAllCategory()
        {
            var categories = await _context.Category.OrderBy(x => x.Name).ToListAsync();

            return categories;
        }

        public IQueryable<Category> GetCategoryFilter()
        {
            return _context.Category.AsQueryable();
        }

        public async Task<Result<Category>> CreateCategory(Category category)
        {
            await _context.Category.AddAsync(category);

            var rowsAffected = await _context.SaveChangesAsync();
            
            return rowsAffected > 0
                ? Result.Success(category)
                : Result.Error("Erro ao criar categoria.");
        }

        public async Task<Result<Category>> GetByIdCategory(Guid categoryId)
        {
            var category = await _context.Category
                .Where(c => c.Id.Equals(categoryId))
                .FirstOrDefaultAsync();

            return category != null ?
                Result.Success(category) :
                Result.Error();
        }

        public async Task<Result<Category>> UpdateCategory(Category updateCategory)
        {
            var category = await _context.Category
              .Where(c => c.Id.Equals(updateCategory.Id))
              .FirstOrDefaultAsync();

            if(category == null)
            {
                return Result.Error("Categoria não encontrada");
            }

            category.Name = updateCategory.Name;
            category.Active = updateCategory.Active;
            category.Type = updateCategory.Type;

            _context.Entry(category).State = EntityState.Modified;

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0
                ? Result.Success(category)
                : Result.Error("Erro ao atualizar a categoria");
        }
    }
}

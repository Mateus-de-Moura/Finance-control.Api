using finance_control.Application.CategoryCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;


namespace finance_control.Application.CategoryCQ.Handler
{
    public class CreateCategoryHandler(ICategoryRepository repository) : IRequestHandler<CreateCategoryCommand, ResponseBase<Category>>
    {
        private readonly ICategoryRepository _repository = repository;
        public async Task<ResponseBase<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Type = (CategoryType)request.Type,
                Active = request.Active,
                UserId = request.UserId,
            };

            var result = await _repository.CreateCategory(category);


            return result.IsSuccess ?
                ResponseBase<Category>.Success(result.Value) :
                ResponseBase<Category>.Fail("Erro ao criar", "Tente novamente", 400);
        }
    }
}

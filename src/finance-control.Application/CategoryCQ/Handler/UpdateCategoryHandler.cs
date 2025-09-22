using finance_control.Application.CategoryCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.CategoryCQ.Handler
{
    public class UpdateCategoryHandler(ICategoryRepository repository) : IRequestHandler<UpdateCategoryCommand, ResponseBase<Category>>
    {
        public async Task<ResponseBase<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {

            if (request.Id == Guid.Empty)            
                return ResponseBase<Category>.Fail("Nenhuma categoria encontrada", "ID inválido", 404);            

            var category = new Category
            {
                Id = request.Id,
                Name = request.Name,
                Active = request.Active,
                Type = (CategoryType)request.Type,
            };

            var result = await repository.UpdateCategory(category);

            return result.IsSuccess
                ? ResponseBase<Category>.Success(result.Value)
                : ResponseBase<Category>.Fail("Falha na Atualização", "Id não econtrado ou atributo com valor incorreto", 400);
        }
    }
}

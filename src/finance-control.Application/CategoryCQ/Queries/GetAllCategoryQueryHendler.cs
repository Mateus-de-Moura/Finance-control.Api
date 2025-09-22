using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetAllCategoryQueryHendler(ICategoryRepository repository) : IRequestHandler<GetAllCategoryQuery, ResponseBase<List<Category>>>
    {       
        public async Task<ResponseBase<List<Category>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await repository.GetAllCategory();

            return categories is not null ?
                ResponseBase<List<Category>>.Success(categories) :
                ResponseBase<List<Category>>.Fail("Nenhum registro encontrado", "Não há nenhuma categoria cadastrada", 404);
        }
    }
}

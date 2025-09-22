using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetByIdCategoryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetByIdCategoryQuery, ResponseBase<Category>>
    {     
        public async Task<ResponseBase<Category>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return ResponseBase<Category>.Fail("Id inválido", "Informe um Id válido", 400);
            }

            var result = await categoryRepository.GetByIdCategory(request.Id);

            return result.IsSuccess ?
                ResponseBase<Category>.Success(result.Value) :
                ResponseBase<Category>.Fail("Erro ao buscar registro", "Não foi encontrado nenhum registro com o Id informado", 404);
        }
    }
}

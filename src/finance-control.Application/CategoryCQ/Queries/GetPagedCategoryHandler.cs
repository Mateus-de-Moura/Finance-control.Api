using AutoMapper;
using finance_control.Application.CategoryCQ.ViewModels;
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using finance_control.Infra.Data.Repositories;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetPagedCategoryHandler(CategoryRepository repository, IMapper mapper) : IRequestHandler<GetPagedCategoryQuery, ResponseBase<PaginatedList<CategoryViewModel>>>
    {
        private readonly CategoryRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<PaginatedList<CategoryViewModel>>> Handle(GetPagedCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.GetCategoryFilter();

            if (!string.IsNullOrEmpty(request.Name))
            {
                result = result.Where(r => r.Name.Contains(request.Name));
            }

            if (Enum.TryParse<CategoryType>(request.Type, true, out var parsedType) &&
                Enum.IsDefined(typeof(CategoryType), parsedType))
            {
                result = result.Where(r => r.Type == parsedType);
            }

            var paginatedList = await result
               .Select(x => _mapper.Map<CategoryViewModel>(x))
              .PaginatedListAsync(request.PageNumber, request.PageSize);


            return paginatedList is not null ?
                ResponseBase<PaginatedList<CategoryViewModel>>.Success(paginatedList) :
                ResponseBase<PaginatedList<CategoryViewModel>>.Fail("Falha ao buscar dados", "Nenhum item localizado", 404);
        }
    }
}

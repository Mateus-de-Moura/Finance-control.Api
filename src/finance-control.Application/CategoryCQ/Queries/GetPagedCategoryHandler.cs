using System.Linq;
using AutoMapper;
using finance_control.Application.CategoryCQ.ViewModels;
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetPagedCategoryHandler(ICategoryRepository repository, IMapper mapper) : IRequestHandler<GetPagedCategoryQuery, ResponseBase<PaginatedList<CategoryViewModel>>>
    {
        private readonly ICategoryRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<PaginatedList<CategoryViewModel>>> Handle(GetPagedCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.GetCategoryFilter();

            if (request.StartDate == null && request.EndDate == null)
            {
                var now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                request.StartDate = firstDayOfMonth;
                request.EndDate = lastDayOfMonth;
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                result = result.Where(r => r.Name.Contains(request.Name));
            }           

            if (request.StartDate != null)
            {
                result = result.Where(x => x.CreatedAt >= request.StartDate);
            }

            if (request.EndDate != null)
            {
                result = result.Where(x => x.CreatedAt <= request.EndDate);
            }

            if (Enum.TryParse<CategoryType>(request.Type, true, out var categoryType))
            {
                result = result.Where(r => r.Type == categoryType);
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

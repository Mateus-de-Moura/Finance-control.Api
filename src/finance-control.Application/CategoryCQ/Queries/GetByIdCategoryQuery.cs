using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetByIdCategoryQuery : IRequest<ResponseBase<Category>>
    {
        public Guid Id { get; set; }
    }
}

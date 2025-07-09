using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using MediatR;

namespace finance_control.Application.CategoryCQ.Commands
{
    public class CreateCategoryCommand : IRequest<ResponseBase<Category>>
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public Guid UserId { get; set; }
    }
}


using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using MediatR;

namespace finance_control.Application.CategoryCQ.Commands
{
    public class UpdateCategoryCommand : IRequest<ResponseBase<Category>>
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}

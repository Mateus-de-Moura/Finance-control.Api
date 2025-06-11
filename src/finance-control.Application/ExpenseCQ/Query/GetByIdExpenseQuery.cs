using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetByIdExpenseQuery : IRequest<ResponseBase<Expenses>>
    {
       public Guid Id { get; set; }
    }
}

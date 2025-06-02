using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class DeleteExpenseCommand : IRequest<ResponseBase<Expenses>>
    {
        public DeleteExpenseCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}

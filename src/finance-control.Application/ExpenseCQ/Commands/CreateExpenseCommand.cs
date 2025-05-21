using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class CreateExpenseCommand : IRequest<ResponseBase<Expenses>>
    {
        public decimal Value { get; set; }

        public DateTime DueDate { get; set; }

        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }
    }
}

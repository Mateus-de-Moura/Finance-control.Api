using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class UpdateExpenseCommand : IRequest<ResponseBase<Expenses>>
    {
        public string Description { get; set; }

        public bool Recurrent { get; set; }

        public bool Active { get; set; }

        public Guid IdExpense { get; set; }

        public decimal Value {  get; set; }

        public DateTime DueDate { get; set; }

        public InvoicesStatus Status {  get; set; }

        public Guid CategoryId { get; set; }
    }
}

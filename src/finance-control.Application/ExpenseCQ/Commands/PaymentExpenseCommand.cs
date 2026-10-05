using finance_control.Application.Response;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class PaymentExpenseCommand : IRequest<ResponseBase<bool>>
    {
        public Guid IdExpense { get; set; }
        public Guid UserId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Value { get; set; }
    }
}

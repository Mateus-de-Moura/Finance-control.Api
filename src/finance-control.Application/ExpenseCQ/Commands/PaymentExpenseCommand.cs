using finance_control.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class PaymentExpenseCommand : IRequest<ResponseBase<bool>>
    {
        public Guid IdExtense { get; set; }

        public IFormFile? ProofFile { get; set; }
    }
}

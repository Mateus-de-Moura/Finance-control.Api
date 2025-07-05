using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class CreateExpenseCommand : IRequest<ResponseBase<Expenses>>
    {
        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal Value { get; set; }

        public DateTime DueDate { get; set; }

        public Guid UserId { get; set; }

        public Guid CategoryId { get; set; }
        public InvoicesStatus Status { get; set; }

        public IFormFile? ProofFile { get; set; }

    }
}

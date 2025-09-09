using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class PaymentExpenseHandler : IRequestHandler<PaymentExpenseCommand, ResponseBase<bool>>
    {
        private readonly FinanceControlContex _context;

        public PaymentExpenseHandler(FinanceControlContex context)
        {
            _context = context;
        }

        public async Task<ResponseBase<bool>> Handle(PaymentExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _context.Expenses.SingleOrDefaultAsync(e => e.Id.Equals(request.IdExtense));

            if(expense is null || expense.IsDeleted)
            {
                return new ResponseBase<bool>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Despesa não encontrada",
                        ErrorDescription = "Despesa não encontrada ou não existe, tente novamente.",
                        HttpStatus = 404
                    },
                    Value = false
                };
            }
            expense.Status = InvoicesStatus.Pago;

            if (request.ProofFile is not null)
            {
                expense.ExpensesComprovant = new ExpensesComprovant{
                    FileName = request.ProofFile.FileName,
                    FileType = request.ProofFile.ContentType,
                    FileData = await ConvertToBytes(request.ProofFile) };
            }

            await _context.SaveChangesAsync();

            return new ResponseBase<bool>
            {
                ResponseInfo = null,
                Value = true
            };

        }

        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}

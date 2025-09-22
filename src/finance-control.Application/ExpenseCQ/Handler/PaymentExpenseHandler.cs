using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class PaymentExpenseHandler(FinanceControlContex context, IConvertFormFileToBytes convert) : IRequestHandler<PaymentExpenseCommand, ResponseBase<bool>>
    {
        public async Task<ResponseBase<bool>> Handle(PaymentExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await context.Expenses.SingleOrDefaultAsync(e => e.Id.Equals(request.IdExtense));

            if (expense is null || expense.IsDeleted)
                return ResponseBase<bool>.Fail("Despesa não encontrada", "Despesa não encontrada ou não existe, tente novamente.", 404);

            expense.Status = InvoicesStatus.Pago;

            if (request.ProofFile is not null)
            {
                expense.ExpensesComprovant = new ExpensesComprovant
                {
                    FileName = request.ProofFile.FileName,
                    FileType = request.ProofFile.ContentType,
                    FileData = await convert.ConvertToBytes(request.ProofFile)
                };
            }

            await context.SaveChangesAsync();

            return ResponseBase<bool>.Success(true);           
        }
    }
}

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
    public class UpdateExpenseHandler(FinanceControlContex context, IConvertFormFileToBytes convert) : IRequestHandler<UpdateExpenseCommand, ResponseBase<Expenses>>
    {
        public async Task<ResponseBase<Expenses>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await context.Expenses
                .Include(e => e.ExpensesComprovant)
                .Where(e => e.Id.Equals(request.IdExpense))
                .FirstOrDefaultAsync();

            if (expense == null)
                return ResponseBase<Expenses>.Fail("Nenhuma despesa encontrada.", "não foi possivel localizar um registro com o Id informado, tente novamente", 404);

            expense.Description = request.Description;
            expense.IsRecurrent = request.Recurrent;
            expense.Active = request.Active;
            expense.Value = request.Value;
            expense.DueDate = request.DueDate;
            expense.Status = request.Status;
            expense.CategoryId = request.CategoryId;

            if (request.Status == InvoicesStatus.Pago && request.ProofFile != null)
            {
                if (expense.ExpensesComprovant != null)
                {
                    expense.ExpensesComprovant.FileName = request.ProofFile.FileName;
                    expense.ExpensesComprovant.FileType = request.ProofFile.ContentType;
                    expense.ExpensesComprovant.FileData = await convert.ConvertToBytes(request.ProofFile);
                }
                else
                {
                    var comprovant = new ExpensesComprovant
                    {
                        FileName = request.ProofFile.FileName,
                        FileType = request.ProofFile.ContentType,
                        FileData = await convert.ConvertToBytes(request.ProofFile)
                    };

                    expense.ExpensesComprovant = comprovant;
                    context.ExpensesComprovant.Add(comprovant);
                }
            }
            else
                expense.ExpensesComprovant = null;

            var rowsAffected = await context.SaveChangesAsync();

            return rowsAffected > 0 ?
                ResponseBase<Expenses>.Success(expense) :
                ResponseBase<Expenses>.Fail("Falha na atualização.", "Nenhuma linha foi afetada. A operação não pôde ser concluída.", 400);

        }
    }
}

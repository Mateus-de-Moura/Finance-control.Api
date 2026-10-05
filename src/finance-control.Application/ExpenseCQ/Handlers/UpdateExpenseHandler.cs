using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Handlers
{
    public class UpdateExpenseHandler(IExpenseRepository repository, IConvertFormFileToBytes convert) : IRequestHandler<UpdateExpenseCommand, ResponseBase<Expenses>>
    {
        public async Task<ResponseBase<Expenses>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.GetById(request.IdExpense);

            if (!result.IsSuccess)
                return ResponseBase<Expenses>.Fail("Nenhuma despesa encontrada.", "não foi possivel localizar um registro com o Id informado, tente novamente", 404);

            var expense = result.Value;

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
                    expense.ExpensesComprovant = new ExpensesComprovant
                    {
                        FileName = request.ProofFile.FileName,
                        FileType = request.ProofFile.ContentType,
                        FileData = await convert.ConvertToBytes(request.ProofFile)
                    };
                }
            }
            else
                expense.ExpensesComprovant = null;

            var updated = await repository.Update(expense);

            return updated.IsSuccess ?
                ResponseBase<Expenses>.Success(expense) :
                ResponseBase<Expenses>.Fail("Falha na atualização.", "Nenhuma linha foi afetada. A operação não pôde ser concluída.", 400);

        }
    }
}

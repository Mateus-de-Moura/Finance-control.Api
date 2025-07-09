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
    public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, ResponseBase<Expenses>>
    {
        private readonly FinanceControlContex _context;
        private readonly IConvertFormFileToBytes _convert;

        public UpdateExpenseHandler(FinanceControlContex context, IConvertFormFileToBytes convert)
        {
            _context = context;
            _convert = convert;
        }

        public async Task<ResponseBase<Expenses>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id.Equals(request.IdExpense))
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return new ResponseBase<Expenses>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Nenhuma despesa encontrada.",
                        ErrorDescription = "não foi possivel localizar um registro com o Id informado, tente novamente",
                        HttpStatus = 404
                    },
                    Value = null,
                };
            }

            expense.Description = request.Description;
            expense.IsRecurrent = request.Recurrent;
            expense.Active = request.Active;
            expense.Value = request.Value;
            expense.DueDate = request.DueDate;
            expense.Status = request.Status;
            expense.CategoryId = request.CategoryId;
            expense.ProofPath = request.Status == InvoicesStatus.Pago && request.ProofFile != null
                     ? await _convert.ConvertToBytes(request.ProofFile)
                     : null;
         
            _context.Entry(expense).State = EntityState.Modified;

            var rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)

                return new ResponseBase<Expenses>
                {
                    ResponseInfo = null,
                    Value = expense,
                };
            else
            {
                return new ResponseBase<Expenses>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Falha na atualização.",
                        ErrorDescription = "Nenhuma linha foi afetada. A operação não pôde ser concluída.",
                        HttpStatus = 400
                    },
                    Value = null
                };

            }
        }
    
    }
}

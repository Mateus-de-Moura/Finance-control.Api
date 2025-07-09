using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, ResponseBase<Expenses>>
    {
        private readonly FinanceControlContex _context;

        public CreateExpenseHandler(FinanceControlContex context)
        {
            _context = context;
        }

        public async Task<ResponseBase<Expenses>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expenses
            {
                Description = request.Description,
                Active = request.Active,
                Value = request.Value,
                DueDate = request.DueDate,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                Status = request.Status,
                ProofPath = request.Status == InvoicesStatus.Pago && request.ProofFile != null
                     ? await ConvertToBytes(request.ProofFile)
                     : null,
            };

            await _context.AddAsync(expense);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                new ResponseBase<Expenses>
                {
                    ResponseInfo = null,
                    Value = expense
                } :

                new ResponseBase<Expenses>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Erro ao cadastrar despesa",
                        ErrorDescription = "Ocorreu um erro ao salvar os dados, tente novamente.",
                        HttpStatus = 404
                    },
                    Value = null
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

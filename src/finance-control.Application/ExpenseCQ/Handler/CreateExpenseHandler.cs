using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class CreateExpenseHandler(IExpenseRepository repository, IConvertFormFileToBytes convert) : IRequestHandler<CreateExpenseCommand, ResponseBase<Expenses>>
    {
        private readonly IExpenseRepository _repository = repository;
        private readonly IConvertFormFileToBytes _convert = convert;
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
                ExpensesComprovant = request.Status == InvoicesStatus.Pago && request.ProofFile != null
                     ? new ExpensesComprovant
                     {
                         FileName = request.ProofFile.FileName,
                         FileType = request.ProofFile.ContentType,
                         FileData = await _convert.ConvertToBytes(request.ProofFile)
                     }
                     : null,
            };

            var result = await _repository.Create(expense);

            return result.IsSuccess ?
                ResponseBase<Expenses>.Success(expense) :
                ResponseBase<Expenses>.Fail("Erro ao cadastrar despesa", "Ocorreu um erro ao salvar os dados, tente novamente.", 404);
        }
    }
}

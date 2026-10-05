using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Handlers
{
    public class DeleteExpenseHandler(IExpenseRepository repository) : IRequestHandler<DeleteExpenseCommand, ResponseBase<Expenses>>
    {
        public async Task<ResponseBase<Expenses>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.Delete(request.Id);

            return result.IsSuccess ?
                ResponseBase<Expenses>.Success(new Expenses()) :
                ResponseBase<Expenses>.Fail("Nenhuma despesa encontrada.",
                "não foi possivel localizar um registro com o Id informado, tente novamente", 404);
        }
    }
}

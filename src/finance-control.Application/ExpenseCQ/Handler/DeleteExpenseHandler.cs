using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class DeleteExpenseHandler(FinanceControlContex context) : IRequestHandler<DeleteExpenseCommand, ResponseBase<Expenses>>
    {           
        public async Task<ResponseBase<Expenses>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var rowsAffected = await context.Expenses.Where(e => e.Id.Equals(request.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(e => e.Active, false)
                .SetProperty(e => e.IsDeleted, true));

            return rowsAffected > 0 ? 
                ResponseBase<Expenses>.Success(new Expenses()) :
                ResponseBase<Expenses>.Fail("Nenhuma despesa encontrada.", 
                "não foi possivel localizar um registro com o Id informado, tente novamente", 404);
        }
    }
}

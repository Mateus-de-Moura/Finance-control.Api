using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetByIdExpenseHandler(FinanceControlContex context) : IRequestHandler<GetByIdExpenseQuery, ResponseBase<Expenses>>
    {
        private readonly FinanceControlContex _context = context;

        public async Task<ResponseBase<Expenses>> Handle(GetByIdExpenseQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<Expenses>.Fail("Id inválido", "Informe um Id válido", 400);

            var expense = await _context.Expenses
                .Where(x => x.Id.Equals(request.Id))
                .FirstOrDefaultAsync();

            return expense != null ?
                ResponseBase<Expenses>.Success(expense) :
                ResponseBase<Expenses>.Fail("Erro ao buscar registro", "Não foi encontrado nenhum registro com o Id informado", 404);
        }
    }
}

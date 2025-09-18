using AutoMapper;
using AutoMapper.QueryableExtensions;
using finance_control.Application.Response;
using finance_control.Domain.Dtos;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Query
{
    public class GetByIdExpenseHandler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetByIdExpenseQuery, ResponseBase<ExpensesDto>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<ExpensesDto>> Handle(GetByIdExpenseQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<ExpensesDto>.Fail("Id inválido", "Informe um Id válido", 400);

            var expense = await _context.Expenses
             .AsNoTracking()
             .Include(e => e.ExpensesComprovant)
             .Where(x => x.Id.Equals(request.Id))
             .ProjectTo<ExpensesDto>(_mapper.ConfigurationProvider)
             .FirstOrDefaultAsync(cancellationToken);         

            return expense != null ?
                ResponseBase<ExpensesDto>.Success(expense) :
                ResponseBase<ExpensesDto>.Fail("Erro ao buscar registro", "Não foi encontrado nenhum registro com o Id informado", 404);
        }
    }
}

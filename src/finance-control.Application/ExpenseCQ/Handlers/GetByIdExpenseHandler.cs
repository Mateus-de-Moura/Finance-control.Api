using AutoMapper;
using AutoMapper.QueryableExtensions;
using finance_control.Application.ExpenseCQ.Queries;
using finance_control.Application.Response;
using finance_control.Domain.Dtos;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Handlers
{
    public class GetByIdExpenseHandler(IExpenseRepository repository, IMapper mapper) : IRequestHandler<GetByIdExpenseQuery, ResponseBase<ExpensesDto>>
    {
        private readonly IExpenseRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<ExpensesDto>> Handle(GetByIdExpenseQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                return ResponseBase<ExpensesDto>.Fail("Id inválido", "Informe um Id válido", 400);

            var expense = await _repository.GetExpensesFilter()
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

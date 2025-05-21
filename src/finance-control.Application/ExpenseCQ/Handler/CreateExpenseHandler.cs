using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;

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
                Value = request.Value,
                DueDate = request.DueDate,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
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
    }
}

using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;

namespace finance_control.Application.ExpenseCQ.Handlers
{
    public class PaymentExpenseHandler(IExpenseRepository repository) : IRequestHandler<PaymentExpenseCommand, ResponseBase<bool>>
    {
        public async Task<ResponseBase<bool>> Handle(PaymentExpenseCommand request, CancellationToken cancellationToken)
        {
            if (request.PaymentDate == default || request.Value <= 0)
                return ResponseBase<bool>.Fail("Dados inválidos", "Informe uma data e um valor maior que zero.", 400);

            var result = await repository.GetById(request.IdExpense);
            var expense = result.Value;

            if (!result.IsSuccess || !expense.UserId.Equals(request.UserId) || expense.IsDeleted)
                return ResponseBase<bool>.Fail("Despesa não encontrada", "Despesa não encontrada ou não existe, tente novamente.", 404);

            expense.Status = InvoicesStatus.Pago;
            expense.PaymentDate = request.PaymentDate;
            expense.Value = request.Value;

            await repository.Update(expense);

            return ResponseBase<bool>.Success(true);
        }
    }
}

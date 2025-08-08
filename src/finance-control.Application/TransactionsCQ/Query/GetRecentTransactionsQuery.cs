using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetRecentTransactionsQuery : IRequest<ResponseBase<List<Transactions>>>
    {
    }
}

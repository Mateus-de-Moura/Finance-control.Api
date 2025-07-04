using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.ViewModels;
using finance_control.Application.TransactionsCQ.ViewModels;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.TransactionsCQ.Query
{
    public class GetPagedTransactionsHandler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetPagedTransactionsQuey, ResponseBase<PaginatedList<TransactionsViewModel>>>
    {
        private readonly FinanceControlContex _contex = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<PaginatedList<TransactionsViewModel>>> Handle(GetPagedTransactionsQuey request, CancellationToken cancellationToken)
        {
            if(request is null)
                return new ResponseBase<PaginatedList<TransactionsViewModel>> { };

            var queryable =  _contex.Transactions
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.UserId.Equals(request.UserId)).AsQueryable();

            var response = await queryable
               .Select(x => _mapper.Map<TransactionsViewModel>(x))
               .PaginatedListAsync(request.PageNumber, request.PageSize);

            if (response == null)
                return ResponseBase<PaginatedList<TransactionsViewModel>>.Fail("Falha ao buscar dados", "Nenhum item localizado", 404);

            return ResponseBase<PaginatedList<TransactionsViewModel>>.Success(response);
        }
    }
}

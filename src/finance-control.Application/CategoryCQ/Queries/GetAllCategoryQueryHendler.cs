using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetAllCategoryQueryHendler(FinanceControlContex context) : IRequestHandler<GetAllCategoryQuery, ResponseBase<List<Category>>>
    {
        private readonly FinanceControlContex _contex = context;

        public async Task<ResponseBase<List<Category>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _contex.Category.ToListAsync(cancellationToken);

            return categories is not null ?
                ResponseBase<List<Category>>.Success(categories) :
                ResponseBase<List<Category>>.Fail("Nenhum registro encontrado", "Não há nenhuma categoria cadastrada", 404);
        }
    }
}

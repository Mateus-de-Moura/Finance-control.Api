using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.DashBoardCQ.ViewModels;
using finance_control.Application.Response;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace finance_control.Application.DashBoardCQ.Query
{
    public class DashboardHandler(FinanceControlContex contex) : IRequestHandler<DashBoardQuery, ResponseBase<DashboardViewModel>>
    {
        private readonly FinanceControlContex _contex = contex;
        public async Task<ResponseBase<DashboardViewModel>> Handle(DashBoardQuery request, CancellationToken cancellationToken)
        {
            var cultureInfo = new System.Globalization.CultureInfo("pt-BR");
            if (request.UserId == Guid.Empty)
                return ResponseBase<DashboardViewModel>.Fail("Id inválido", "Id do usuário inválido ou vazio, tente novamente", 400);

            try
            {
                var totalRevenuesToMonth = await _contex.Revenues
                .Where(x => x.UserId.Equals(request.UserId) && x.Date.Value.Month == DateTime.Now.Month &&
                 x.Date.Value.Year == DateTime.Now.Year && x.Active).ToListAsync();

                var expensesToMonth = await _contex.Expenses
                    .Where(x => x.UserId.Equals(request.UserId) && x.DueDate.Month == DateTime.Now.Month &&
                     x.DueDate.Year == DateTime.Now.Year && x.Active).ToListAsync();

                var dashboard = new DashboardViewModel
                {
                    Revenues = totalRevenuesToMonth.Sum(x => x.Value).ToString("C", cultureInfo),
                    Expenses = expensesToMonth.Sum(x => x.Value).ToString("C", cultureInfo),
                    ExpensesOpen = expensesToMonth.Where(x => x.Status != InvoicesStatus.Pago).Sum(x => x.Value).ToString("C", cultureInfo),
                    Wallet = (totalRevenuesToMonth.Sum(x => x.Value) - expensesToMonth.Where(x => x.Status == InvoicesStatus.Pago).Sum(x => x.Value)).ToString("C", cultureInfo),
                };

                return new ResponseBase<DashboardViewModel> { ResponseInfo = null, Value = dashboard };


            }
            catch (Exception ex)
            {
                return ResponseBase<DashboardViewModel>.Fail("Erro", ex.Message, 400);
            }         
        }
    }
}

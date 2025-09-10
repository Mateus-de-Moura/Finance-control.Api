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
                var currentYear = DateTime.Now.Year;

                var totalRevenuesToMonth = await _contex.Revenues
                .Where(x => x.UserId.Equals(request.UserId) && x.Date.Value.Month == DateTime.Now.Month &&
                 x.Date.Value.Year == DateTime.Now.Year && x.Active).ToListAsync();

                var expensesToMonth = await _contex.Expenses
                    .Where(x => x.UserId.Equals(request.UserId) && x.DueDate.Month == DateTime.Now.Month &&
                     x.DueDate.Year == DateTime.Now.Year && x.Active).ToListAsync();

                var transactions = await _contex.Transactions.Where(x => x.UserId.Equals(request.UserId) 
                     && x.TransactionDate.Month == DateTime.Now.Month &&
                     x.TransactionDate.Year == DateTime.Now.Year && x.Active).ToListAsync();

                var totalTransactiosRevenues = transactions.Where(x => x.Type == TypesEnum.Receitas).ToList().Sum(x => x.Value);
                var totalTransactiosExpenses = transactions.Where(x => x.Type == TypesEnum.Despesas).ToList().Sum(x => x.Value);

                var totalRevenues = totalTransactiosRevenues + totalRevenuesToMonth.Sum(x => x.Value);
                var totalExpenses = totalTransactiosExpenses + expensesToMonth.Sum(x => x.Value);
                var totalExpensesPaid = expensesToMonth.Where(x => x.Status == InvoicesStatus.Pago).Sum(x => x.Value) +
                    transactions
                    .Where(x => x.Type == TypesEnum.Despesas && x.Status == StatusPaymentEnum.Confirmado)
                    .ToList()
                    .Sum(x => x.Value);

                var dashboard = new DashboardViewModel
                {
                    Revenues = totalRevenues.ToString("C", cultureInfo),
                    Expenses = totalExpenses.ToString("C", cultureInfo),
                    ExpensesOpen = expensesToMonth.Where(x => x.Status != InvoicesStatus.Pago).Sum(x => x.Value).ToString("C", cultureInfo),
                    Wallet = (totalRevenues - totalExpensesPaid).ToString("C", cultureInfo)
                };

                dashboard.MonthlySummary = new List<MonthlyDataViewModel>();

                for (int month = 1; month <= 12; month++)
                {
                    var monthRevenues = await _contex.Revenues
                        .Where(x => x.UserId == request.UserId &&
                                    x.Date.HasValue &&
                                    x.Date.Value.Month == month &&
                                    x.Date.Value.Year == DateTime.Now.Year &&
                                    x.Active)
                        .SumAsync(x => (decimal?)x.Value) ?? 0;

                    var monthExpenses = await _contex.Expenses
                        .Where(x => x.UserId == request.UserId &&
                                    x.DueDate.Month == month &&
                                    x.DueDate.Year == DateTime.Now.Year &&
                                    x.Active)
                        .SumAsync(x => (decimal?)x.Value) ?? 0;

                    var monthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", cultureInfo);

                    dashboard.MonthlySummary.Add(new MonthlyDataViewModel
                    {
                        Month = cultureInfo.TextInfo.ToTitleCase(monthName),
                        Revenues = monthRevenues,
                        Expenses = monthExpenses
                    });
                }

                return new ResponseBase<DashboardViewModel> { ResponseInfo = null, Value = dashboard };


            }
            catch (Exception ex)
            {
                return ResponseBase<DashboardViewModel>.Fail("Erro", ex.Message, 400);
            }         
        }
    }
}

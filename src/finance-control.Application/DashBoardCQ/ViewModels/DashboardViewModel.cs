using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Entity;

namespace finance_control.Application.DashBoardCQ.ViewModels
{
    public class DashboardViewModel
    {
        public string Revenues { get; set; }
        public string Expenses { get; set; }
        public string Wallet {  get; set; }
        public string ExpensesOpen { get; set; }

        public List<MonthlyDataViewModel> MonthlySummary { get; set; } = new List<MonthlyDataViewModel>();
    }
}

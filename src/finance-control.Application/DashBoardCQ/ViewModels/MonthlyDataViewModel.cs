using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.DashBoardCQ.ViewModels
{
    public class MonthlyDataViewModel
    {
        public string Month { get; set; } 
        public decimal Revenues { get; set; }
        public decimal Expenses { get; set; }
    }
}

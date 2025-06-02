using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Entity;

namespace finance_control.Application.RevenuesCQ.ViewModels
{
    public class RevenuesViewModel
    {
        public bool Active { get; set; }
        public string? Description { get; set; }

        public string Value { get; set; }

        public string Date { get; set; }

     public string Category { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.TransactionsCQ.ViewModels
{
    public class TransactionsViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Active { get; set; }
        public string TransactionDate { get; set; }
        public string Type { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Observation { get; set; }
    }
}

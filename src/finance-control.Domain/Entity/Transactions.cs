using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Enum;

namespace finance_control.Domain.Entity
{
    public class Transactions : BaseEntity
    {
       public DateTime TransactionDate { get; set; }
       public int Type { get; set; } 
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public int PaymentMethod { get; set; } 
        public int Status { get; set; } 
        public string? Observation {  get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }
}

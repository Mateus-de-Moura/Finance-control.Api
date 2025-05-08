using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Domain.Entity
{
    public class Revenues : BaseEntity
    {
        public bool Active { get; set; }
        public string? Description { get; set; }
       
        public decimal Value { get; set; }

        public DateTime? Date { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }  
    }
}

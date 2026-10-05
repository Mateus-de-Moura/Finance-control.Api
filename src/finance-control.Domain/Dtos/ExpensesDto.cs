using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;

namespace finance_control.Domain.Dtos
{
    public class ExpensesDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Active { get; set; }
        public decimal Value { get; set; }

        public bool IsRecurrent { get; set; }

        public DateTime DueDate { get; set; }

        public InvoicesStatus Status { get; set; }

        public Guid CategoryId { get; set; }

        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public User User { get; set; }

        public Category Category { get; set; }

        public bool IsDeleted { get; set; }

        public string? File { get; set; }
        public string? FileType { get; set; }
    }
}

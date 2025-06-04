using finance_control.Domain.Enum;

namespace finance_control.Domain.Entity
{
    public class Expenses : BaseEntity
    {
        public decimal Value { get; set; }

        public DateTime DueDate { get; set; }

        public InvoicesStatus Status { get; set; }

        public Guid CategoryId { get; set; }

        public Guid UserId { get; set; }
        public string Description { get; set; }
        public User User { get; set; }

        public Category Category { get; set; }

        public bool IsDeleted { get; set; }

        public byte[]? ProofPath { get; set; }
    }
}

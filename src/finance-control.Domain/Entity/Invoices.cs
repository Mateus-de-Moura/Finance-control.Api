using finance_control.Domain.Enum;

namespace finance_control.Domain.Entity
{
    public class Invoices
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public Guid CategoryId { get; set; }
        public InvoicesStatus Status { get; set; }
        public byte[]? ProofOfPaymenyt { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Category Category { get; set; }
    }
}

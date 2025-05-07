using System.ComponentModel.DataAnnotations;

namespace finance_control.Domain.Entity
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}

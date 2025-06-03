using System.ComponentModel.DataAnnotations;

namespace BackgroundService.Entity
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Active { get; set; }
    }
}

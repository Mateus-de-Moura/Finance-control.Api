using System.ComponentModel.DataAnnotations;

namespace finance_control.Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? Surname { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public string? PasswordSalt { get; set; }

        [Required]
        public string? UserName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }

        public Guid AppRoleId { get; set; }      

        public virtual PhotosUsers PhotosUsers { get; set; }
        public virtual AppRole Role { get; set; }

        public List<Expenses> Expenses { get; set; }
    }
}

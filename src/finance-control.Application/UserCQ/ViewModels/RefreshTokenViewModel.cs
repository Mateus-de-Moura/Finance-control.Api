using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.UserCQ.ViewModels
{
    public record RefreshTokenViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? TokenJwt { get; set; }
        public string? RefreshToken { get; set; }
        public string Photo { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }
    }
}

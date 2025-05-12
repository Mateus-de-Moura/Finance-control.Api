
using finance_control.Domain.Entity;

namespace finance_control.Application.UserCQ.ViewModels
{
    public class UserViewModel
    {    
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }   
        public string? UserName { get; set; }
        public string? RoleName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Domain.Entity
{
    public class LoginLocationData : BaseEntity
    {
        public bool IsSuccess { get; set; }      
        public string EmailRequest { get; set; }
        public DateTime AccessDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ip { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

    } 
}

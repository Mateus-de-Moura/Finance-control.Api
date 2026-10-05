using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundService.Entity
{
    public class LoginLocationData : BaseEntity
    {
        public string EmailRequest { get; set; }
        public DateTime AccessDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ip { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }

    } 
}

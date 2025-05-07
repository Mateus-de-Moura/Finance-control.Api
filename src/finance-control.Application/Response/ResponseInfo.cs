using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.Response
{
    public record ResponseInfo
    {
        public string? Title { get; set; }
        public string? ErrorDescription { get; set; }
        public int HttpStatus { get; set; }

    }
}

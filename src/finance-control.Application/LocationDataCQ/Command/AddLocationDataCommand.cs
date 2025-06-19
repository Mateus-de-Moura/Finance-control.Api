using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.LocationDataCQ.Command
{
    public class AddLocationDataCommand : IRequest<ResponseBase<LoginLocationData>>
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Ip { get; set; }
        public string Email { get; set; }
        public string Platform { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
    }
}

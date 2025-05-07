using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.Response
{
    public record ResponseBase<T>
    {
        public ResponseInfo? ResponseInfo { get; set; }
        public T? Value { get; set; }
    }
}

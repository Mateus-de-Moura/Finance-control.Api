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

        public static ResponseBase<T> Fail(string title, string description, int statusCode) => new()
        {
            Value = default,
            ResponseInfo = new ResponseInfo
            {
                Title = title,
                ErrorDescription = description,
                HttpStatus = statusCode
            }
        };

        public static ResponseBase<T> Success(T value) => new()
        {          
            ResponseInfo = null,
            Value = value
        };
    }
}

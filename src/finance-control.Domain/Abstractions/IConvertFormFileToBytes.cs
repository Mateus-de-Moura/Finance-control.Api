using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace finance_control.Domain.Abstractions
{
    public interface IConvertFormFileToBytes
    {
        Task<byte[]> ConvertToBytes(IFormFile file);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace finance_control.Services
{
    public class ConvertFormFileToBytes : IConvertFormFileToBytes
    {
        public async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}


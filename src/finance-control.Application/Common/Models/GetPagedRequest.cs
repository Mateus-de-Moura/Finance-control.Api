using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.Common.Models
{
    public sealed record GetPagedRequest(int PageNumber = 1, int PageSize = 10, string Description = null);

}

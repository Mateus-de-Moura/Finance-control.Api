using finance_control.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.Common.Models
{
    public sealed record GetPagedRequestExpense(
        int PageNumber = 1, 
        int PageSize = 10, 
        Guid? UserId = null, 
        Guid? CategoryId = null, 
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        InvoicesStatus? Status = null,
        string Description = null
        );
    
}

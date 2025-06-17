using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.CategoryCQ.Commands
{
    public class UpdateCategoryCommand : IRequest<ResponseBase<Category>>
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}

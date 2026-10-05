using finance_control.Application.CategoryCQ.ViewModels;
using finance_control.Application.Common.Models;
using finance_control.Application.Response;
using MediatR;

namespace finance_control.Application.CategoryCQ.Queries
{
    public class GetPagedCategoryQuery :  IRequest<ResponseBase<PaginatedList<CategoryViewModel>>>
    {
        public Guid UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Type { get; set; }
        public string Name { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

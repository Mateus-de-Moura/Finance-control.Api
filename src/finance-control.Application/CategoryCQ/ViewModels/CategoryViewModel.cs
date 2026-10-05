using finance_control.Domain.Enum;

namespace finance_control.Application.CategoryCQ.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }        
        public CategoryType Type { get; set; }

    }
}

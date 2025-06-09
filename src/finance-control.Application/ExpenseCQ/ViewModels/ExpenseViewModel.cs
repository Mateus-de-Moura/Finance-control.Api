using finance_control.Domain.Entity;
using finance_control.Domain.Enum;


namespace finance_control.Application.ExpenseCQ.ViewModels
{
    public class ExpenseViewModel
    {
        //public ExpenseViewModel(Guid id, string description, string categoryName, string value, string dueDate, InvoicesStatus status)
        //{
        //    Id = id;
        //    Description = description;
        //    CategoryName = categoryName;
        //    Value = value;
        //    DueDate = dueDate;
        //    Status = status;
        //}

        public Guid Id { get; set; }
      
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string Value { get; set; }

        public string DueDate { get; set; }

        public InvoicesStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                if (Status == InvoicesStatus.Pago)
                    return "Pago";

                if (DateTime.TryParse(DueDate, out var parsedDueDate))
                {
                    if (parsedDueDate.Date < DateTime.Now.Date)
                        return "Vencido";
                }

                return "Pendente";
            }
        }
    }
}

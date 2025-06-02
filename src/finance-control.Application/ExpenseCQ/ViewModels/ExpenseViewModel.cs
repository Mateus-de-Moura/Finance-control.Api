using finance_control.Domain.Entity;
using finance_control.Domain.Enum;


namespace finance_control.Application.ExpenseCQ.ViewModels
{
    public class ExpenseViewModel
    {
        public ExpenseViewModel(Guid id, decimal value, DateTime dueDate, InvoicesStatus status)
        {
            Id = id;
            Value = value;
            DueDate = dueDate;
            Status = status;
        }

        public Guid Id { get; set; }
      
        public decimal Value { get; set; }

        public DateTime DueDate { get; set; }

        public InvoicesStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                if (Status == InvoicesStatus.Pago)
                    return "Pago";

                if (DueDate > DateTime.Now)
                    return "Vencido";

                return "Pendente";
            }
        }

        public static ExpenseViewModel FromEntity(Expenses entity)
            => new ExpenseViewModel(entity.Id,  entity.Value, entity.DueDate, entity.Status);
    }
}

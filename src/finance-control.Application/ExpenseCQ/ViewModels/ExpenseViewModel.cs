using finance_control.Domain.Entity;
using finance_control.Domain.Enum;


namespace finance_control.Application.ExpenseCQ.ViewModels
{
    public class ExpenseViewModel
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
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

                if (Status == InvoicesStatus.Vencido)
                {
                    return "Vencido";
                }

                return "Pendente";
            }
        }
    }
}

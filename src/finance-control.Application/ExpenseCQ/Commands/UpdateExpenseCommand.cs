using Azure;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.ExpenseCQ.Commands
{
    public class UpdateExpenseCommand : IRequest<ResponseBase<Expenses>>
    {
        public Guid IdExpense { get; set; }

        public decimal Value {  get; set; }

        public DateTime DueDate { get; set; }
    }
}

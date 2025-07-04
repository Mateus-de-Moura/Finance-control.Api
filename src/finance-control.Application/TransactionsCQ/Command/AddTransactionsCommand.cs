using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using MediatR;

namespace finance_control.Application.TransactionsCQ.Command
{
    public class AddTransactionsCommand : IRequest<ResponseBase<Transactions>>
    {
        public DateTime TransactionDate { get; set; }
        public int Type { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public int PaymentMethod { get; set; }
        public int Status { get; set; }
        public string? Observation { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}

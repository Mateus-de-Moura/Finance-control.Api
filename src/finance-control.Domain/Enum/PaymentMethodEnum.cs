using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace finance_control.Domain.Enum
{
    public class PaymentMethodEnum: SmartEnum<PaymentMethodEnum, int>
    {
        public static readonly PaymentMethodEnum Dinheiro = new(nameof(Dinheiro), 0);
        public static readonly PaymentMethodEnum Cartao = new(nameof(Cartao), 1);
        public static readonly PaymentMethodEnum Pix = new(nameof(Pix), 2);
        public PaymentMethodEnum(string name, int value): base(name, value)
        {
                
        }
    }
}

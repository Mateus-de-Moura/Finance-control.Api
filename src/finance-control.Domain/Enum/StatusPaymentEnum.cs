using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace finance_control.Domain.Enum
{
    public class StatusPaymentEnum : SmartEnum<StatusPaymentEnum, int>
    {
        public static readonly StatusPaymentEnum Pendente = new(nameof(Pendente), 0);
        public static readonly StatusPaymentEnum Confirmado = new(nameof(Confirmado), 1);
        public static readonly StatusPaymentEnum Cancelado = new(nameof(Cancelado), 2);
        public StatusPaymentEnum(string name, int value) : base(name, value) 
        {                
        }
    }
}

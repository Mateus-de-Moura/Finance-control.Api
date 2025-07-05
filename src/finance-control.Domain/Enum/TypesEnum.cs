using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace finance_control.Domain.Enum
{
    public  class TypesEnum : SmartEnum<TypesEnum, int>
    {
        public static readonly TypesEnum Receitas = new(nameof(Receitas), 0);
        public static readonly TypesEnum Despesas = new(nameof(Despesas), 1);
        public TypesEnum(string name, int value) : base(name, value)
        {                
        }
    }
}

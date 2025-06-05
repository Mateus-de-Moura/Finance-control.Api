using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundService.Enum
{
    public enum InvoicesStatus
    {
        [Description("Pendente")]
        Pendente,

        [Description("Pago")]
        Pago,

        [Description("Vencido")]
        Vencido
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Domain.Entity
{
    public class ExpensesComprovant : BaseEntity
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }      
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundService.Entity
{
    public class Notify : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ExpensesId { get; set; }
        public string Message { get; set; }
        public bool WasRead { get; set; }
        public DateTime ReadDate { get; set; }
        public string Priority { get; set; }
        public Guid UserId { get; set; }

        public  User User { get; set; }
        public Expenses Expenses { get; set; }
    }
}

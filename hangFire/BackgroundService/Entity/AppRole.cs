﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundService.Entity
{
    public sealed class AppRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> appUsers { get; set; }
    }
}

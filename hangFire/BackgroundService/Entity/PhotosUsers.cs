﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundService.Entity
{
    public class PhotosUsers : BaseEntity
    {
        public Guid UserId { get; set; }

        public byte[] PhotoUser { get; set; }        

        public virtual User User { get; set; }


    }
}

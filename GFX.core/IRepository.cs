﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace GFX.Core
{
    public interface IRepository
    {
        DbContext Context { get; set; }
    }
}

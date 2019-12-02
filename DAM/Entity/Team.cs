﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAM.Attribute;

namespace DAM.Entity
{
    [DAMEntity("Team")]
    class Team
    {
        [PrimaryKey]
        public long Id { get; set; }
        [PrimaryKey]
        public string Name { get; set; }

    }
}

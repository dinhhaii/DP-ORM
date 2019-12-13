using DAM.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Entity
{
    [Table("Organization")]
    class Organization
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

using DAM.AttributeEntity;
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
        [Column]
        public long Id { get; set; }

        [Column]
        public string Name { get; set; }

        [OneToMany(refClassType: typeof(Client), mappedBy: "organization")]
        public List<object> client { get; set; }

    }
}

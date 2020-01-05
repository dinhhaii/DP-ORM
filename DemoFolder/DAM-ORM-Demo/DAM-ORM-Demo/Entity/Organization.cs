using DAM.AttributeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_ORM_Demo.Entity
{
    [Table("Organization")]
    public class Organization
    {
        [PrimaryKey]
        [Column]
        public long Id { get; set; }

        [Column]
        public string Name { get; set; }

        [OneToMany(refClassType: typeof(Client), mappedBy: "organization")]
        public List<object> client { get; set; }

        [ManyToMany(refClassType: typeof(Team), mappedBy: "organizations")]
        [JoinTable(name: "Organization_Team", joinColumn: "Organization_Id", refJoinColumn: "Team_Id")]
        public List<object> teams { get; set; }
    }
}

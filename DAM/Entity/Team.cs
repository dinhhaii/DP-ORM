﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAM.AttributeEntity;

namespace DAM.Entity
{
    [Table("Team")]
    class Team
    {
        [PrimaryKey]
        [Column]
        public long Id { get; set; }

        [Column]
        public string Name { get; set; }

        [OneToMany(refClassType:typeof(Client),mappedBy:"team")]
        public List<object> Clients { get; set; }

        [ManyToMany(refClassType: typeof(Organization), mappedBy: "teams")]
        [JoinTable(name: "Organization_Team", refJoinColumn: "Organization_Id", joinColumn: "Team_Id")]
        public List<object> organizations { get; set; }
    }
}

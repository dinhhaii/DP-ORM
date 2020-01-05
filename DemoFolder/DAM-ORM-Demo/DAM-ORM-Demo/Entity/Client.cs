using DAM.AttributeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_ORM_Demo.Entity
{
    [Table("Client")]
    public class Client
    {
        [PrimaryKey]
        [Column("Id")]
        public long Id { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [ManyToOne]
        [JoinColumn("Id")]
        public Organization organization { get; set; }

        [ManyToOne]
        [JoinColumn("teamId")]
        public Team team { get; set; }

        [OneToOne(refClassType: typeof(ClientDetail), mappedBy: "client")]
        public ClientDetail clientDetail { get; set; }
    }
}


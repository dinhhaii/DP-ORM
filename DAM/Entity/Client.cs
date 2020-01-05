using DAM.AttributeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Entity
{
    [Table("Client")]
    class Client
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

        [OneToOne(refClassType:typeof(ClientDetail),mappedBy:"client")]
        public ClientDetail clientDetail { get; set; }


        public override string ToString()
        {
            string result = $"Client Id:{Id}  Username:{Username}  Password:{Password}"
            + $" Organization ID : " + (organization == null ? "null" : $"{organization.Id}")
            + $" Organization Name : " + (organization== null ? "null" : $"{organization.Name}")
            + $" Team ID : " + (team == null ? "null" : $"{team.Id}")
            + $" Team Name : " + (team == null ? "null" : $"{team.Name}");


            return result;
        }
    }
}

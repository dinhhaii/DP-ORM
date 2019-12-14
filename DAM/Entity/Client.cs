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
        [PrimaryKey] public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int OrganizationId { get; set; }
        public int TeamId { get; set; }
        [OneToOne] public Organization organization { get; set; }
        [OneToOne] public Team team { get; set; }
    }
}

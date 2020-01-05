using DAM.AttributeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Entity
{
    [Table("ClientDetail")]
    class ClientDetail
    {
        [Column]
        public string address { get; set; }

        [OneToOne]
        [PrimaryKey]
        [JoinColumn("id")]
        public Client client { get; set; }
    }
}

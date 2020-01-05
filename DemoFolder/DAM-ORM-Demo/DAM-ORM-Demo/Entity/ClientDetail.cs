using DAM.AttributeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_ORM_Demo.Entity
{
    [Table("ClientDetail")]
    public class ClientDetail
    {
        [Column]
        public string address { get; set; }

        [OneToOne]
        [PrimaryKey]
        [JoinColumn("id")]
        public Client client { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.AttributeEntity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class OneToOne : System.Attribute
    {
        public Type refClassType { get; set; }
        public string mappedBy { get; set; }
        public OneToOne(Type refClassType,string mappedBy) {
            this.mappedBy = mappedBy;
            this.refClassType = refClassType;
        }
        public OneToOne()
        {
            mappedBy = "";
            refClassType = null;
        }
    }
}

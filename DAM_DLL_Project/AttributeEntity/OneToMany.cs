using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.AttributeEntity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class OneToMany : System.Attribute
    {
        public Type refClassType { get; set; }
        public string mappedBy { get; set; }

        public OneToMany(Type refClassType, string mappedBy)
        {
            this.refClassType = refClassType;
            this.mappedBy = mappedBy;
        }
        
        
    }
}

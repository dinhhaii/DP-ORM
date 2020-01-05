using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAM.AttributeEntity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class JoinColumn : System.Attribute
    {
        private string name { get; set; }

        public JoinColumn(string _name)
        {
            name = _name;
        }
        public JoinColumn()
        {
            name = "";
        }

        public override string ToString()
        {
            return name;
        }
    }
}

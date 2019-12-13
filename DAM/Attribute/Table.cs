using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    class Table : System.Attribute
    {
        private string name { get; set; }

        public Table(string _name)
        {
            name = _name;
        }
    }
}

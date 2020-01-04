using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAM.AttributeEntity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    class JoinTable : System.Attribute
    {
        public string Name { get; set; }
        public string JoinColumn { get; set; }
        public string RefJoinColumn { get; set; }


        public JoinTable(string name, string joinColumn, string refJoinColumn)
        {
            Name = name;
            JoinColumn = joinColumn;
            RefJoinColumn = refJoinColumn;
        }

    }
}

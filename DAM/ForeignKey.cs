using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class ForeignKey
    {
        public string tableName { get; set; }
        public string refTableName { get; set; }
        public List<string> foreignKeys { get; set; }

        public ForeignKey()
        {
        }
        public ForeignKey(string tblName)
        {
            tableName = tblName;
            foreignKeys = new List<string>();
        }
        public int AddForeignKey(string keyName)
        {
            foreignKeys.Add(keyName);
            return foreignKeys.Count;
        }
    }
}

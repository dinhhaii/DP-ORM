using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class DataTable
    {
        public List<Dictionary<string,object>> dataTable { get; set; }

        public DataTable() {
            dataTable = new List<Dictionary<string, object>>();
        }

        public List<object> GetColumn(string name)
        {
            List<object> result = new List<object>();
            foreach(var item in dataTable)
            {
                result.Add(item[name]);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public class DataTable
    {
        public List<Dictionary<string,object>> dataTable { get; set; }
        public List<string> columnsName { get; set; }
        public string _tableName { get; set; }
        public DataTable() {
            _tableName = "";
            dataTable = new List<Dictionary<string, object>>();
            columnsName = new List<string>();
        }
        public DataTable(string tableName)
        {
            _tableName = tableName;
            dataTable = new List<Dictionary<string, object>>();
            columnsName = new List<string>();
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

        public void AddRow(List<object> values)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int i = 0; i< columnsName.Count;i++)
            {
                row.Add(columnsName[i], values[i]);
            }
            dataTable.Add(row);
        }
    }
}

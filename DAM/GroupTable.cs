using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public class GroupTable
    {
        public List<Dictionary<string,object>> dataTable { get; set; }
        public List<string> columnsName { get; set; }
        public string _tableName { get; set; }
        public GroupTable() {
            _tableName = "";
            dataTable = new List<Dictionary<string, object>>();
            columnsName = new List<string>();
        }
        public GroupTable(string tableName)
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
            if (!dataTable.Contains(row))
            {
                dataTable.Add(row);
            }
        }

        public void AddRow(Dictionary<string, object> row)
        {
            bool isNotExists = true;
            foreach (Dictionary<string, object> item in dataTable)
            {
                bool isEqual = true;
                foreach(string key in item.Keys)
                {
                    if (!DeepEquals(item[key], row[key]))
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                {
                    isNotExists = false;
                    break;
                }
            }

            if (isNotExists)
            {
                dataTable.Add(row);
            }
        }
        public bool DeepEquals(object obj, object another)
        {
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var result = true;
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) result = false;
            }

            Type type = obj.GetType();
            if (type == typeof(string) || type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(long) || type == typeof(DateTime))
            {
                result = (obj == another);
            }

            return result;
        }
    }
}

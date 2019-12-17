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
        public GroupTable(GroupTable groupTable)
        {
            dataTable = groupTable.dataTable;
            columnsName = groupTable.columnsName;
            _tableName = groupTable._tableName;
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
            AddRow(row);
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

        public GroupTable Count(string colName)
        {
            string tableName = string.Format("COUNT({0})", colName);
            if (columnsName.Contains(colName)) columnsName.Remove(colName);

            foreach (string name in columnsName)
            {
                if (name != columnsName.Last()) tableName += name + "_";
                else tableName += name + "_Group";
            }

            _tableName = tableName;
            columnsName.Add("Count");

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Count", 1);
            }

            for (int i = 0; i < dataTable.Count - 1; i++)
            {
                for (int j = i + 1; j < dataTable.Count; j++)
                {
                    bool isEqual = true;
                    foreach (string name in columnsName)
                    {
                        if (!DeepEquals(dataTable[i][name], dataTable[j][name]))
                        {
                            isEqual = false;
                            break;
                        }
                    }

                    if (isEqual)
                    {
                        dataTable.RemoveAt(j);
                        dataTable[i]["Count"] = Convert.ToInt32(dataTable[i]["Count"]) + 1;
                        j--;
                    }
                }
            }

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Remove(colName);
            }

            return this;
        }

        public GroupTable Average(string colName)
        {
            string tableName = string.Format("AVERAGE({0})", colName);
            if (columnsName.Contains(colName)) columnsName.Remove(colName);

            foreach (string name in columnsName)
            {
                if (name != columnsName.Last()) tableName += name + "_";
                else tableName += name + "_Group";
            }

            _tableName = tableName;
            columnsName.Add("Average");

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Sum", row[colName]);
                row.Add("Count", 1);
            }

            for (int i = 0; i < dataTable.Count - 1; i++)
            {
                for (int j = i + 1; j < dataTable.Count; j++)
                {
                    bool isEqual = true;
                    foreach (string name in columnsName)
                    {
                        if (!DeepEquals(dataTable[i][name], dataTable[j][name]))
                        {
                            isEqual = false;
                            break;
                        }
                    }

                    if (isEqual)
                    {
                        dataTable[i]["Count"] = Convert.ToInt32(dataTable[i]["Count"]) + 1;
                        dataTable[i]["Sum"] = Convert.ToDouble(dataTable[i]["Sum"]) + Convert.ToDouble(dataTable[j][colName]);
                        dataTable.RemoveAt(j);
                        j--;
                    }
                }
            }

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Average", Convert.ToDouble(row["Sum"]) / Convert.ToDouble(row["Count"]));
                row.Remove(colName);
                row.Remove("Sum");
                row.Remove("Count");
            }

            return this;
        }

        public GroupTable Sum(string colName)
        {
            string tableName = string.Format("SUM({0})", colName);
            if (columnsName.Contains(colName)) columnsName.Remove(colName);

            foreach (string name in columnsName)
            {
                if (name != columnsName.Last()) tableName += name + "_";
                else tableName += name + "_Group";
            }

            _tableName = tableName;
            columnsName.Add("Sum");

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Sum", row[colName]);
            }

            for (int i = 0; i < dataTable.Count - 1; i++)
            {
                for (int j = i + 1; j < dataTable.Count; j++)
                {
                    bool isEqual = true;
                    foreach (string name in columnsName)
                    {
                        if (!DeepEquals(dataTable[i][name], dataTable[j][name]))
                        {
                            isEqual = false;
                            break;
                        }
                    }

                    if (isEqual)
                    {
                        dataTable[i]["Sum"] = Convert.ToDouble(dataTable[i]["Sum"]) + Convert.ToDouble(dataTable[j][colName]);
                        dataTable.RemoveAt(j);
                        j--;
                    }
                }
            }

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Remove(colName);
            }
            return this;

        }

        public GroupTable Min(string colName)
        {
            string tableName = string.Format("MIN({0})", colName);
            if (columnsName.Contains(colName)) columnsName.Remove(colName);

            foreach (string name in columnsName)
            {
                if (name != columnsName.Last()) tableName += name + "_";
                else tableName += name + "_Group";
            }

            _tableName = tableName;
            columnsName.Add("Min");

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Min", row[colName]);
            }

            for (int i = 0; i < dataTable.Count - 1; i++)
            {
                for (int j = i + 1; j < dataTable.Count; j++)
                {
                    bool isEqual = true;
                    foreach (string name in columnsName)
                    {
                        if (!DeepEquals(dataTable[i][name], dataTable[j][name]))
                        {
                            isEqual = false;
                            break;
                        }
                    }

                    if (isEqual)
                    {
                        dataTable[i]["Min"] = Convert.ToInt32(dataTable[i]["Min"]) > Convert.ToInt32(dataTable[j][colName]) ? dataTable[j][colName] : dataTable[i]["Min"];
                        dataTable.RemoveAt(j);
                        j--;
                    }
                }
            }

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Remove(colName);
            }
            return this;
        }

        public GroupTable Max(string colName)
        {
            string tableName = string.Format("MAX({0})", colName);
            if (columnsName.Contains(colName)) columnsName.Remove(colName);

            foreach (string name in columnsName)
            {
                if (name != columnsName.Last()) tableName += name + "_";
                else tableName += name + "_Group";
            }

            _tableName = tableName;
            columnsName.Add("Max");

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Add("Max", row[colName]);
            }

            for (int i = 0; i < dataTable.Count - 1; i++)
            {
                for (int j = i + 1; j < dataTable.Count; j++)
                {
                    bool isEqual = true;
                    foreach (string name in columnsName)
                    {
                        if (!DeepEquals(dataTable[i][name], dataTable[j][name]))
                        {
                            isEqual = false;
                            break;
                        }
                    }

                    if (isEqual)
                    {
                        dataTable[i]["Max"] = Convert.ToInt32(dataTable[i]["Max"]) < Convert.ToInt32(dataTable[j][colName]) ? dataTable[j][colName] : dataTable[i]["Max"];
                        dataTable.RemoveAt(j);
                        j--;
                    }
                }
            }

            foreach (Dictionary<string, object> row in dataTable)
            {
                row.Remove(colName);
            }
            return this;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public class SqlDAMEntity : DAMEntity
    {
        public SqlDAMEntity(IDatabase database, string tableName) : base(database, tableName)
        {

        }
        public SqlDAMEntity(IDatabase database) : base(database)
        {

        }
        public override List<object> ToList()
        {
            return _database.GenerateListFromTable(TableName);
        }

        public override object FindById(object key)
        {
            try
            {
                Dictionary<string, object> primaryKey = new Dictionary<string, object>();

                var listprimaryKeyName = _database.FindPrimaryKeyName(TableName);
                primaryKey.Add(listprimaryKeyName[0], key);
                return _database.FindByPrimaryKey(primaryKey, TableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        public override int Update(List<object> listObj)
        {
            int result = 0;
            foreach (var item in listObj)
            {
                if (_database.UpdateObjectToDB(item) > 0)
                {
                    result += 1;
                }

            }
            return result;
        }

        public override object Update(object obj)
        {
            return _database.UpdateObjectToDB(obj); ;
        }

        public override int Add(List<object> listObj)
        {
            int result = 0;
            foreach (var item in listObj)
            {
                if (_database.AddObjectToDB(item) > 0)
                {
                    result += 1;
                }
            }
            return result;
        }

        public override int Add(object obj)
        {
            return _database.AddObjectToDB(obj);
        }

        public override int Delete(List<object> listObj)
        {
            int result = 0;
            foreach (var item in listObj)
            {
                if (_database.DeleteObjectInDB(item) > 0)
                {
                    result += 1;
                }
            }
            return result;
        }

        public override int Delete(object obj)
        {
            return _database.DeleteObjectInDB(obj);
        }

        public override GroupTable Group(params string[] groupedColumnNames)
        {
            List<object> table = _database.GenerateListFromTable(TableName);
            GroupTable result = new GroupTable();
            string groupedTableName = "";
            List<string> columnsName = new List<string>();

            foreach (string columnName in groupedColumnNames)
            {
                if (columnName == groupedColumnNames.Last())
                    groupedTableName += columnName + "_Group";
                else
                    groupedTableName += columnName + "_";

                columnsName.Add(columnName);
            }

            result.columnsName = columnsName;
            result._tableName = groupedTableName;

            Type entityType = _database.GetTypeByClassName(TableName);
            PropertyInfo[] properties = entityType.GetProperties();

            //Remove properties aren't grouped
            foreach (PropertyInfo property in properties)
            {
                if (!groupedColumnNames.Contains(property.Name))
                {
                    properties = properties.Where(p => p != property).ToArray();
                }
            }

            //Save all objects are grouped to DataTable
            foreach (object obj in table)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (PropertyInfo property in properties)
                {
                    row.Add(property.Name, property.GetValue(obj));
                }
                result.AddRow(row);
            }

            return result;
        }

        public override GroupTable Count(string columnName, params string[] groupedColumnNames)
        {
            string[] concat = new string[groupedColumnNames.Length + 1];
            concat[0] = columnName;
            if (groupedColumnNames != null && groupedColumnNames.Length != 0)
            {
                groupedColumnNames.CopyTo(concat, 1);
            }

            GroupTable group = Group(concat);
            group.Count(columnName);
            return group;
        }

        public override GroupTable Average(string columnName, params string[] groupedColumnNames)
        {
            string[] concat = new string[groupedColumnNames.Length + 1];
            concat[0] = columnName;
            if (groupedColumnNames != null && groupedColumnNames.Length != 0)
            {
                groupedColumnNames.CopyTo(concat, 1);
            }

            GroupTable group = Group(concat);
            group.Average(columnName);
            return group;
        }

        public override GroupTable Sum(string columnName, params string[] groupedColumnNames)
        {
            string[] concat = new string[groupedColumnNames.Length + 1];
            concat[0] = columnName;
            if (groupedColumnNames != null && groupedColumnNames.Length != 0)
            {
                groupedColumnNames.CopyTo(concat, 1);
            }

            GroupTable group = Group(concat);
            group.Sum(columnName);
            return group;
        }

        public override GroupTable Min(string columnName, params string[] groupedColumnNames)
        {
            string[] concat = new string[groupedColumnNames.Length + 1];
            concat[0] = columnName;
            if (groupedColumnNames != null && groupedColumnNames.Length != 0)
            {
                groupedColumnNames.CopyTo(concat, 1);
            }

            GroupTable group = Group(concat);
            group.Min(columnName);
            return group;
        }

        public override GroupTable Max(string columnName, params string[] groupedColumnNames)
        {
            string[] concat = new string[groupedColumnNames.Length + 1];
            concat[0] = columnName;
            if (groupedColumnNames != null && groupedColumnNames.Length != 0)
            {
                groupedColumnNames.CopyTo(concat, 1);
            }

            GroupTable group = Group(concat);
            group.Max(columnName);
            return group;
        }
    }
}

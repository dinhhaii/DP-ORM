using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class SqlClientDB : IDatabase
    {
        private static string connectionString { get; set; }
        private static SqlConnection connection { get; set; }
        public SqlClientDB(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public static SqlConnection GetConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed || connection == null)
            {
                connection = new SqlConnection(connectionString);
            }
            return connection;
        }

        public static void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public List<object> GenerateListFromTable(string tableName)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();
            List<object> tables = new List<object>();

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery().Select("*").From(tableName);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    List<string> PK = FindPrimaryKeyName(tableName);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        if (FK.Count == 0)
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                object propertyReader = reader[property.Name];
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Dictionary<string, object> pKey = new Dictionary<string, object>();
                            foreach(string PKName in PK)
                            {
                                pKey.Add(PKName, reader[PKName]);
                            }
                            entity = FindByPrimaryKey(pKey, tableName);
                        }
                        tables.Add(entity);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return tables;
        }


        public object FindByPrimaryKey(Dictionary<string, object> primaryKeys, string tableName)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();
               
                if (primaryKeys != null)
                {
                    string condition = "";
                    foreach (KeyValuePair<string, object> item in primaryKeys)
                    {
                        if (item.Key != null && item.Value != DBNull.Value)
                        {
                            condition += item.Value.GetType() == typeof(string) ? string.Format("{0} = '{1}'", item.Key, item.Value) : string.Format("{0} = {1}", item.Key, item.Value);
                            if (!item.Equals(primaryKeys.Last()))
                            {
                                condition += " and ";
                            }
                        }
                    }
                    if (condition != "")
                        query = SqlClientQuery.InitQuery().Select("*").From(tableName).Where(condition);
                }
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.PropertyType != typeof(string) && property.PropertyType != typeof(int) && property.PropertyType != typeof(long) && 
                            property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(float) && property.PropertyType != typeof(double))
                            {
                                Dictionary<string, object> pKey = new Dictionary<string, object>();
                                foreach(ForeignKey fk in FK)
                                {
                                    if (fk.refTableName == property.PropertyType.Name)
                                    {
                                        for (int i = 0; i < fk.foreignKeys.Count; i++)
                                        {
                                            string pkRefTable = fk.primaryKeysOfRefTable[i];
                                            string fkTable = fk.foreignKeys[i];

                                            pKey.Add(pkRefTable, reader[fkTable]);
                                        }
                                        object obj = FindByPrimaryKey(pKey, fk.refTableName);
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (null != propEntity && propEntity.CanWrite)
                                        {
                                            propEntity.SetValue(entity, obj, null);
                                        }

                                    }
                                }                                
                            }
                            else
                            {
                                object propertyReader = reader[property.Name];
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }
                            }
                        }
                        return entity;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return null;
        }

        public int UpdateObjectToDB(object obj)
        {
            
            string tableName = "";
            Dictionary<string, object> valueUpdate = new Dictionary<string, object>();
            Attribute[] attributes = Attribute.GetCustomAttributes(obj.GetType());
            Type entityType = Type.GetType(obj.GetType().ToString());
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (var attr in attributes)
            {
                tableName = attr.ToString();
            }
            List<string> columnName = getColumnnameTable(tableName);

            foreach (PropertyInfo property in properties)
            {
                object propertyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                Type typeProperty = propertyValue.GetType();
                foreach (var item in columnName)
                {
                    if(item==property.Name)
                    {
                        valueUpdate.Add(property.Name, propertyValue);
                    }
                }

            }
            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();
                var conditionValue = FindPrimaryKeyName(tableName);
                string condition = "";
                foreach(var item in conditionValue)
                {
                    condition += valueUpdate[item].GetType() == typeof(string) ? string.Format("{0} = '{1}'", item, valueUpdate[item]) : string.Format("{0} = {1}", item, valueUpdate[item]);
                    if (!item.Equals(conditionValue.Last()))
                    {
                        condition += " and ";
                    }
                }
                query.Update(tableName).Set(valueUpdate).Where(condition);
            }
            return 0;
        }

        public List<string> getColumnnameTable(string tableName)
        {
            List<string> result = new List<string>();
            using (connection = new SqlConnection(connectionString))
            {
                string queryString = @"SELECT Column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName + "'";
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    ForeignKey foreignKey = new ForeignKey(tableName);

                    while (reader.Read())
                    {
                        for(int i = 0;i<reader.FieldCount;i++)
                        {
                            result.Add((string)reader.GetValue(i));
                        } 
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
            return result;
        }
        public List<ForeignKey> FindForeignKeyOfTable(string tableName)
        {
            List<ForeignKey> result = new List<ForeignKey>();

            using (connection = new SqlConnection(connectionString))
            {
                string queryString = string.Format(@"SELECT object_name(f.referenced_object_id) RefTableName, col_name(fc.parent_object_id,fc.parent_column_id) ForeignKey FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id WHERE f.parent_object_id = object_id('{0}')", tableName);
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    ForeignKey foreignKey = new ForeignKey(tableName);

                    while (reader.Read())
                    {
                        string reftableName = (reader["RefTableName"] as string).Trim();
                        string fk = (reader["ForeignKey"] as string).Trim();

                        if (reftableName != null && fk != null)
                        {
                            if (reftableName != foreignKey.refTableName)
                            {
                                foreignKey = new ForeignKey(tableName);
                                List<string> pks = FindPrimaryKeyName(reftableName);
                                foreignKey.refTableName = reftableName;
                                foreignKey.primaryKeysOfRefTable = pks;
                                result.Add(foreignKey);
                            }

                            result[result.Count - 1].AddForeignKey(fk);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        public string GetRefTableName(List<ForeignKey> foreignKeys, string propertyName)
        {
            foreach (ForeignKey item in foreignKeys)
            {
                if (item.foreignKeys.Contains(propertyName))
                {
                    return item.refTableName;
                }
            }
            return null;
        }

        public List<string> FindPrimaryKeyName(string tableName)
        {
            List<string> result = new List<string>();

            using (connection = new SqlConnection(connectionString))
            {
                string queryString = string.Format(@"SELECT COLUMN_NAME as PrimaryKey FROM dam.INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME LIKE '{0}' AND CONSTRAINT_NAME LIKE 'PK%'", tableName);
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string pk = (reader["PrimaryKey"] as string).Trim();
                        if (pk != null)
                            result.Add(pk);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }
    }
}

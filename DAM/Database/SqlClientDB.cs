using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class SqlClientDB : Database
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
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            object propertyReader = reader[property.Name];
                            if (propertyReader != null)
                            {
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

        public List<object> FindByPrimaryKey(List<string> primaryKeys, string tableName)
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
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            object propertyReader = reader[property.Name];
                            if (propertyReader != null)
                            {
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
                                foreignKey.refTableName = reftableName;
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
    }
}

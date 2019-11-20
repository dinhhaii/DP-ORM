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
        public object FindById(string tableName, long id)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();
            List<object> result = new List<object>();
            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery().Select("*").From(tableName).Where("Id = @Id");
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Id", id));
                (query as SqlClientQuery).AddParameter(parameters);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
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
                        result.Add(entity);
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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class SqlClientDB : Database
    {
        private static string connectionString { get; set; }
        public SqlClientDB(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public void Execute(Query query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("|{0}|\n|{1}|\n|{2}|\n|{3}|", reader["Id"], (reader["Username"] as string).Trim(), (reader["Name"] as string).Trim(), (reader["Password"] as string).Trim());
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}

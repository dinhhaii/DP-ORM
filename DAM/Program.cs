using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace DAM
{
    class Program
    {
        static void Main(string[] args)
        {
            string queryString = "SELECT * FROM Client WHERE Id = @Id";
            query(queryString);
            Console.ReadKey();

        }

        static public void query(string queryString)
        {
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=dam;Integrated Security=True";
            int value = 2;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, connection);
                sqlCommand.Parameters.AddWithValue("@Id", value);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("|{0}|\n|{1}|\n|{2}|\n|{3}|", reader[0], (reader[1] as string).Trim(), reader[2], reader[3]);
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

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
            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=dam;Integrated Security=True";
            SqlClientDB sqlDatabase = new SqlClientDB(connectionString);
            List<ForeignKey> foreignKeys = sqlDatabase.FindForeignKeyOfTable("Client");

            Console.ReadKey();

        }
    }
}

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
            Database sqlDatabase = new SqlClientDB(connectionString);
            Query query = SqlClientQuery.InitQuery().Select("*").From("client");
            sqlDatabase.Execute(query);

            Console.ReadKey();

        }
    }
}

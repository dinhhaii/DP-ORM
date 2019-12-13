using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAM.Entity;

namespace DAM
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string connectionString = @"Data Source=DESKTOP-8JKPIIU\SQLEXPRESS2017;Initial Catalog=Dam;Integrated Security=True";
            IDatabase sqlDatabase = new SqlClientDB(connectionString);
            //List<ForeignKey> foreignKeys = sqlDatabase.FindForeignKeyOfTable("Client");

            Dictionary<string, object> pk = new Dictionary<string, object>();
            pk.Add("Id", 3);

            List<object> list = sqlDatabase.GenerateListFromTable("Client");
            List<object> list1 = sqlDatabase.GenerateListFromTable("Client");
            int k = 0;
            Console.ReadKey();

        }
    }
}

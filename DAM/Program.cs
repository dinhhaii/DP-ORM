using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAM.Entity;
using DAM.EntityQuery;

namespace DAM
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string connectionString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=Dam;Integrated Security=True";
            IDatabase sqlDatabase = new SqlClientDB(connectionString);
            //List<ForeignKey> foreignKeys = sqlDatabase.FindForeignKeyOfTable("Client");
            SqlDAMEntity sqlDAMEntity = new SqlDAMEntity(sqlDatabase, "Client");
            
            Dictionary<string, object> pk = new Dictionary<string, object>();
            pk.Add("Id", 3);
            Client client = sqlDAMEntity.FindById(1) as Client;
            sqlDAMEntity.Delete(client);
            Client clientAdd = new Client();
            clientAdd.Username = "Nguyen Duy Hau";
            clientAdd.Password = "1";
            sqlDAMEntity.Add(clientAdd);
            List<object> list = sqlDatabase.GenerateListFromTable("Client");
            List<object> list1 = sqlDatabase.GenerateListFromTable("Client");
            int k = 0;
            Console.ReadKey();

        }
    }
}

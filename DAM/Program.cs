using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAM.Entity;
using System.Reflection;
using DAM.Factory;

namespace DAM
{
    class Program
    {
        static void Main(string[] args)
        {

            string thanhhaiConnectionString = @"Data Source=DESKTOP-2GTF1EB\SQLEXPRESS; Initial Catalog = dam; Integrated Security = True";
            string hauConnectionString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=dam;Integrated Security=True";
            //string haiConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=dam;Integrated Security=True";
            //IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(hauConnectionString);
            IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(hauConnectionString);
            IDatabase database2 = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(thanhhaiConnectionString);

            DAMEntity clientEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Client");
            
            //List<object> clients = clientEntity.ToList();



            Client client = new Client();
            client.Username = "hai";
            List<object> clientList = clientEntity.FindByObject(client);
            Console.WriteLine("First List:");
            foreach (Client c in clientList) Console.WriteLine(c.ToString());



            Query query1 = new SqlClientQuery()
                .Select("*")
                .From("Client")
                .Where("Username LIKE '%hai%'");

            List<object> list1 = clientEntity.FindByQuery(query1);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Second List:");
            foreach (Client c in list1) Console.WriteLine(c.ToString());




            Query query2 = new SqlClientQuery()
                .Select("teamId, Count(*) as Amount")
                .From("Client")
                .Where("teamId>1")
                .GroupBy("teamId")
                .Having("teamId<10");
                       
            List<object> list2 = clientEntity.FindByQuery(query2);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Third List:");
            foreach (object c in list2) Console.WriteLine(c.ToString());



            Console.ReadKey();
            
            
        }
    }
}

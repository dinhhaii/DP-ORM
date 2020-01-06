using DAM;
using DAM.Factory;
using DAM_ORM_Demo.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAM_ORM_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string typeName = Assembly.GetEntryAssembly().GetTypes().ToList().SingleOrDefault(x => x.Name == "Client").FullName;
            Type entityType = Type.GetType(typeName);
            string hauConnectionString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=dam;Integrated Security=True";
            //string haiConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=dam;Integrated Security=True";
            IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(hauConnectionString);

            DAMEntity clientEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Client");

            Console.WriteLine("Total client in Database before Add: ");
            List<object> clients = (List<object>)(Object)clientEntity.ToList();
            Console.WriteLine(clients.Count.ToString());
            Console.WriteLine("Add Client To DB: ");
            Client client = new Client();
            client.Username = "Nguyen Duy Hau";
            client.Password = "123";
            if(clientEntity.Add(client)>0)
            {
                Console.WriteLine("Add Client To DB Successful. ");
            }
            Console.WriteLine("Total client in Database after Add: ");
            List<object> clientsAfterAdd = (List<object>)(Object)clientEntity.ToList();
            Console.WriteLine(clientsAfterAdd.Count.ToString());

            Console.WriteLine("Update Client in Database:");

            Client clientUpdate =  clientEntity.FindById(2) as Client;
            Console.WriteLine("Client before updated:");
            Console.WriteLine("Username: " + clientUpdate.Username + ", " + "Password: " +clientUpdate.Password);
            clientUpdate.Username = "Nguyen Duy Hau Updated";
            clientEntity.Update(clientUpdate);
            Console.WriteLine("Client after updated:");
            Console.WriteLine("Username: " + clientUpdate.Username + ", " + "Password: " + clientUpdate.Password);

            clientEntity.Delete(clientsAfterAdd[clientsAfterAdd.Count-1]);


            Console.WriteLine("Total client in Database before Delete: ");
            List<object> clientsAfterDelete = (List<object>)(Object)clientEntity.ToList();
            Console.WriteLine(clientsAfterDelete.Count.ToString());
            Console.ReadKey();
        }
    }
}

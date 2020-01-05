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
            string thanhhaiConnectionString = @"Data Source=DESKTOP-2GTF1EB\SQLEXPRESS; Initial Catalog = dam; Integrated Security = True";

            //IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(hauConnectionString);
            IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(thanhhaiConnectionString);

            DAMEntity sqlDAMEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Organization");

            DAMEntity clientEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Client");
            //List<object> clients = (List<object>)(Object)clientEntity.ToList();
               
            //Console.WriteLine(clients[0]);
            Console.ReadKey();
        }
    }
}

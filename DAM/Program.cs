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

            DAMEntity clientEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Client");
            List<object> clients = clientEntity.ToList();

            Console.WriteLine("HELLO");
            Console.ReadKey();

        }
    }
}

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
            string hauConnectionString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=Dam;Integrated Security=True";
            //string haiConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=dam;Integrated Security=True";
            //IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(hauConnectionString);
            IDatabase database = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDatabase(thanhhaiConnectionString);

            DAMEntity sqlDAMEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Organization");
            Organization client = sqlDAMEntity.FindById(1) as Organization;

            DAMEntity sqlDAMEntity2 = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Team");
            Team team = sqlDAMEntity2.FindById(1) as Team;
            
            Console.WriteLine("HELLO");
            Console.ReadKey();

        }
    }
}

﻿using System;
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

            DAMEntity sqlDAMEntity = DAMFactory.GetDAMFactory(DatabaseType.Sql).InitDAMEntity(database, "Client");

            //Dictionary<string, object> pk = new Dictionary<string, object>();
            //pk.Add("Id", 3);
            Client client = sqlDAMEntity.FindById(3) as Client;
            //sqlDAMEntity.Delete(client);
            Client clientAdd = new Client();
            Organization organization = new Organization();
            organization.Id = 1;
            Team team = new Team();
            team.Id = 1;
            client.Username = "Nguyen Duy Hau DepTrai";
            client.Password = "12";
            client.organization = organization;
            client.team = team;
            //sqlDAMEntity.Update(client);
            //List<object> list = sqlDatabase.GenerateListFromTable("Client");
            //List<object> list1 = sqlDatabase.GenerateListFromTable("Client");
            //GroupTable grouptable = sqlDAMEntity.Min("Id", "team");
            

            Console.WriteLine("HELLO");
            Console.ReadKey();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.EntityQuery
{
    public class SqlDAMEntity : DAMEntity
    {
        public SqlDAMEntity(IDatabase database, string tableName) :base(database,tableName)
        {
            
        }
        public SqlDAMEntity(IDatabase database) : base(database)
        {

        }
        public override List<object> ToList()
        {
            return _database.GenerateListFromTable(TableName);
        }

        public override object FindById(object key)
        {
            try
            {
                Dictionary<string, object> primaryKey = new Dictionary<string, object>();

                var listprimaryKeyName = _database.FindPrimaryKeyName(TableName);
                primaryKey.Add(listprimaryKeyName[0], key);
                return _database.FindByPrimaryKey(primaryKey, TableName);
            }
            catch(Exception ex)
            {

            }

            return 0;
        }

        //Return number of object are updated 
        public override int Update(List<object> listObj)
        {
            int result = 0;
            foreach(var item in listObj)
            {
               if(_database.UpdateObjectToDB(item)>0)
               {
                    result += 1;
               }

            }
            return result;
        }

        public override object Update(object obj)
        {           
            return _database.UpdateObjectToDB(obj); ;
        }

        public override int Add(List<object> listObj)
        {
            int result = 0;
            foreach(var item in listObj)
            {
                if(_database.AddObjectToDB(item)>0)
                {
                    result += 1;
                }
            }
            return result;
        }

        public override int Add(object obj)
        {
            return _database.AddObjectToDB(obj);
        }

        public override int Delete(List<object> listObj)
        {
            int result = 0;
            foreach (var item in listObj)
            {
                if (_database.DeleteObjectInDB(item) > 0)
                {
                    result += 1;
                }
            }
            return result;
        }

        public override int Delete(object obj)
        {
            return _database.DeleteObjectInDB(obj);
        }
    }
}

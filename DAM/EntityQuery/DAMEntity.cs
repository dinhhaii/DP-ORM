using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.EntityQuery
{
    public class DAMEntity
    {
        protected IDatabase _database;
        public string TableName { get; set; }
        public void setIDatabase(IDatabase db)
        {
            _database = db;
        }
        public DAMEntity(IDatabase database)
        {
            _database = database;
        }

        public DAMEntity(IDatabase database, string tableName)
        {
            _database = database;
            TableName = tableName;
        }

        public virtual List<object> ToList()
        {
            return null;
        }

        public virtual object FindById(object key)
        {
            return null;
        }

        public virtual int Update(List<object> listObj)
        {
            return 0;
        }

        public virtual object Update(object obj)
        {
            return 0;
        }

        public virtual int Add(List<object> listObj)
        {
            return 0;
        }

        public virtual int Add(object obj)
        {
            return 0;
        }

        public virtual int Delete(List<object> listObj)
        {
            return 0;
        }

        public virtual int Delete(object obj)
        {
            return 0;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public abstract class DAMEntity
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

        public abstract List<object> ToList();

        public abstract object FindById(object key);

        public abstract int Update(List<object> listObj);

        public abstract object Update(object obj);

        public abstract int Add(List<object> listObj);

        public abstract int Add(object obj);

        public abstract int Delete(List<object> listObj);

        public abstract int Delete(object obj);

        public abstract GroupTable Group(params string[] groupedColumnNames);
    }
}

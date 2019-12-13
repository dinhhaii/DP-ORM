using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.EntityQuery
{
    class SqlDAMEntity : DAMEntity
    {
        public SqlDAMEntity(IDatabase database, string tableName) :base(database,tableName)
        {
            
        }
        public SqlDAMEntity(IDatabase database) : base(database)
        {

        }
        public override List<object> ToList()
        {
            return null;
        }

        public override object FindById(int key)
        {
            return null;
        }

        public override int Update(List<object> listObj)
        {
            return 0;
        }

        public override object Update(object obj)
        {
            return 0;
        }

        public override int Add(List<object> listObj)
        {
            return 0;
        }

        public override int Add(object obj)
        {
            return 0;
        }

        public override int Delete(List<object> listObj)
        {
            return 0;
        }

        public override int Delete(object obj)
        {
            return 0;
        }
    }
}

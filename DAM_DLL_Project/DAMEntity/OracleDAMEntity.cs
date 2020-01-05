using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public class OracleDAMEntity : DAMEntity
    {
        public OracleDAMEntity(IDatabase database, string tableName) : base(database, tableName)
        {

        }

        public OracleDAMEntity(IDatabase database) : base(database)
        {

        }

        public override List<object> ToList()
        {
            throw new NotImplementedException();
        }

        public override object FindById(object key)
        {
            throw new NotImplementedException();
        }
        public override List<object> FindByQuery(Query query)
        {
            throw new NotImplementedException();
        }
        public override List<object> FindByObject(object obj)
        {
            throw new NotImplementedException();
        }
        public override int Update(List<object> listObj)
        {
            throw new NotImplementedException();
        }

        public override object Update(object obj)
        {
            throw new NotImplementedException();
        }

        public override int Add(List<object> listObj)
        {
            throw new NotImplementedException();
        }

        public override int Add(object obj)
        {
            throw new NotImplementedException();
        }

        public override int Delete(List<object> listObj)
        {
            throw new NotImplementedException();
        }

        public override int Delete(object obj)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Group(params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Count(string columnName, params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Average(string columnName, params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Sum(string columnName, params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Min(string columnName, params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }

        public override GroupTable Max(string columnName, params string[] groupedColumnNames)
        {
            throw new NotImplementedException();
        }
    }
}

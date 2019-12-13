using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.EntityQuery
{
    public class OracleDAMEntity : DAMEntity
    {
        public OracleDAMEntity(IDatabase database, string tableName) : base(database, tableName)
        {

        }

        public OracleDAMEntity(IDatabase database) : base(database)
        {

        }
    }
}

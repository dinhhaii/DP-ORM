using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Factory
{
    public class SqlClientFactory : DAMAbstractFactory
    {
        public override DAMEntity InitDAMEntity(IDatabase database, string tableName)
        {
            return new SqlDAMEntity(database, tableName);
        }
        public override IDatabase InitDatabase(string conn)
        {
            return new SqlClientDB(conn);
        }
        public override Query InitQuery()
        {
            return base.InitQuery();
        }
    }
}

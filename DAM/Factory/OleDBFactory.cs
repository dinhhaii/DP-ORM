using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Factory
{
    public class OleDBFactory : DAMAbstractFactory
    {
        public override DAMEntity InitDAMEntity(IDatabase database, string tableName)
        {
            return base.InitDAMEntity(database, tableName);
        }
        public override IDatabase InitDatabase(string _conn)
        {
            return base.InitDatabase(_conn);
        }
        public override Query InitQuery()
        {
            return base.InitQuery();
        }
    }
}

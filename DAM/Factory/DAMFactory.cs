using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Factory
{
    public class DAMFactory
    {
        public static DAMAbstractFactory GetDAMFactory(DatabaseType type)
        {
            switch(type)
            {
                case DatabaseType.Sql:
                    return new SqlClientFactory();
                case DatabaseType.Oracle:
                    return new OracleFactory();
                case DatabaseType.Ole:
                    return new OleDBFactory();
                default:
                    return null;
            }
        }
    }
}

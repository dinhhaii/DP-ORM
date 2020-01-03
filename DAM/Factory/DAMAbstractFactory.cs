using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM.Factory
{
    public abstract class DAMAbstractFactory
    {
        public virtual IDatabase InitDatabase(string _conn)
        {
            return null;
        }

        public virtual DAMEntity InitDAMEntity(IDatabase database , string tableName)
        {
            return null;
        }
        public virtual Query InitQuery()
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    interface Query
    {
        Query select(string property);
        Query from(string property);
        Query where(string property);

        string generateQueryString();

    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    interface Query
    {
        Query Select(string property);
        Query From(string property);
        Query Where(string property);
        Query Update(string property);
        Query Set(Dictionary<string, object> valueUpdate);
        Query GroupBy(string property);
        Query Having(string condition);
        string QueryString();
        object GenerateCommand(object connection);
    }
}

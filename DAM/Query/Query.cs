using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public interface Query
    {
        Query Select(string property);
        Query From(string property);
        Query Where(string property);
        Query Update(string property);
        Query Set(Dictionary<string, object> valueUpdate);
        Query GroupBy(string property);
        Query Having(string condition);
        Query Insert(string table);
        Query Values(Dictionary<string, object> valueAdd);

        Query Delete(string table);

        string QueryString();
        object GenerateCommand(object connection);
    }
}

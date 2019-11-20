using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class QuerySqlClient : Query
    {
        private string queryString { get; set; }

        public QuerySqlClient(string query)
        {
            queryString = query;
        }
        public Query select(string property)
        {
            queryString += string.Format("SELECT {0} ", property);
            return this;
        }

        public Query from(string table)
        {
            queryString += string.Format("FROM {0} ", table);
            return this;
        }

        public Query where(string condition)
        {
            queryString += string.Format("WHERE {0} ", condition);
            return this;
        }

        public string generateQueryString()
        {
            return queryString;
        }
    }
}

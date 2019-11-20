using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class SqlClientQuery : Query
    {
        private string queryString { get; set; }

        public SqlClientQuery()
        {
            queryString = "";
        }

        public SqlClientQuery(string query)
        {
            queryString = query;
        }

        static public Query InitQuery()
        {
            return new SqlClientQuery();
        }

        static public Query InitQuery(string query)
        {
            return new SqlClientQuery(query);
        }
        public Query Select(string property)
        {
            queryString += string.Format("SELECT {0} ", property);
            return this;
        }

        public Query From(string table)
        {
            queryString += string.Format("FROM {0} ", table);
            return this;
        }

        public Query Where(string condition)
        {
            queryString += string.Format("WHERE {0} ", condition);
            return this;
        }

        public string QueryString()
        {
            return queryString;
        }

        public object GenerateCommand(object connection)
        {
            return new SqlCommand(queryString, (SqlConnection)connection);
        }
    }
}

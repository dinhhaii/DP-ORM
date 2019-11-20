using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    class SqlClientDB : Database
    {
        private string connectionString { get; set; }
        public SqlClientDB(string _connectionString)
        {
            connectionString = _connectionString;
        }
    }
}

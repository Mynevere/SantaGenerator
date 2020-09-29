using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaGenerator
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = @"DESKTOP-RNC5KLO\SQLEXPRESS";

            string database = "santa_db ";
            string username = "sa";
            string password = "sa";

            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }
    }
}

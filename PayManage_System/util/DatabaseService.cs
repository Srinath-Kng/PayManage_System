using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PayManage_System.util
{
    internal class DatabaseService
    {
        static string connectionString = "Server=LAGLOP\\SQLEXPRESS;Database=CASE_STUDY;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
 
        public static void CloseConnection(SqlConnection con)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}

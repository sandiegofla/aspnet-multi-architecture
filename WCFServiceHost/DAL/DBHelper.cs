using System.Configuration;
using System.Data.SQLite;

namespace WCFServiceHost.DAL
{
    public class DBHelper
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["ConexaoSQLite"].ConnectionString;

        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}

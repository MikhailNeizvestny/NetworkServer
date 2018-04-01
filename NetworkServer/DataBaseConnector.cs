using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace NetworkServer
{
    class DataBaseConnector
    {
        static string connStr = "server=localhost;user=root;database=screenshotmaker;password=;";
        MySqlConnection conn = new MySqlConnection(connStr);
        
        public void insertImage(string fileName, string fileFormat)
        {
            conn.Open();
            string sql = "INSERT INTO screenshotmaker.images ( id , filename , format )" +
                         "VALUES( NULL, '"+ fileName +"', '" + fileFormat + "' ); ";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.ExecuteScalar();
            conn.Close();
        }
    }
}

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
                         "VALUES( NULL, @fileName, @fileFormat ); ";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@fileName", MySqlDbType.VarChar).Value = fileName;
            command.Parameters.Add("@fileFormat", MySqlDbType.VarChar).Value = fileFormat;
            command.ExecuteScalar();
            conn.Close();
        }

        public int getIdByName(string fileName)
        {
            conn.Open();
            string sql = "SELECT id FROM screenshotmaker.images " +
                "WHERE filename = @fileName";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.Add("@fileName", MySqlDbType.VarChar).Value = fileName;
            int id = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return id;
        }
    }
}

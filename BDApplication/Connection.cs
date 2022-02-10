using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDApplication
{
    public class Connection
    {
        public MySqlConnection ConnectToDatabase()
        {
            string stringConnection = "server=localhost;database=pensionnaire; uid=root; pwd=;";
            MySqlConnection connection = new MySqlConnection(stringConnection);
            try
            {
                connection.Open();
            }
            catch (Exception exception)
            {
                Console.WriteLine("connection failed" + exception.Message);
            }
            return connection;
        }

    }
}

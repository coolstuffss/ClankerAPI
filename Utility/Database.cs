using System;
using MySql.Data.MySqlClient;

/*
 * !!! in this class the transactions are not implemented yet !!!
 */

namespace ClankerAPI.Utility
{
    class Database
    {
        private MySqlConnection Conn { get; set; }
        private MySqlCommand Cmd { get; set; }
        private MySqlDataReader DataReader { get; set; }
        private string Server { get; }
        private string DB { get; }
        private string User { get; }
        private string Password { get; }

        /**
         * this construction will create a new MySql connection
         * and it will create a MySql command for the database operations
         * */
        public Database(string server, string db, string user, string password)
        {
            Server = server;
            DB = db;
            User = user;
            Password = password;
            Conn = new MySqlConnection($"server={Server}; database={DB}; UID={user}; password={password}");
            Cmd = new MySqlCommand();
        }

        public Database(string server, string db)
        {
            Server = server;
            DB = db;
            User = "root";
            Password = String.Empty;
            Conn = new MySqlConnection($"server={Server}; database={DB}; UID=root; password={String.Empty}");
            Cmd = new MySqlCommand();
        }

        /**
         * this will open the connection to the database
         * be sure to handle the MySqlException exception
         * */
        public void Open()
        {
            try { Conn.Open(); }
            catch (MySqlException) { throw; }
        }

        /**
         * be sure to invoke the Open() method or it will throw an exception
         * this method will execute all the commands for updating and insert data
         * the method will not close the connection
         * usage: INSERT INTO <tablename> (<parameters>) VALUES (<values>)
         * */
        public void SendNonQuery(string query)
        {
            Cmd.CommandText = query;
            Cmd.Connection = Conn;
            Cmd.ExecuteNonQuery();
        }

        /**
         * be sure to invoke the Open() method or it will throw an exception
         * this method will execute a commands for selecting data from your database
         * and return a MySqlDataReader so you have to handle the output of the data reader by yourself
         * */
        public MySqlDataReader SendQuery(string query)
        {
            Cmd.CommandText = query;
            Cmd.Connection = Conn;
            DataReader = Cmd.ExecuteReader();

            return DataReader;
        }

        /**
         * this will close the connection to the database
         * be sure to handle the MySqlException exception
         * */
        public void Close()
        {
            try{ Conn.Close(); }
            catch { throw; }
        }

        public override string ToString() => $"Server: {Server} - Database name: {DB} - User: {User} - Passsword: {Password}";
    }
}
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DotNetCoreConsoleApp
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Stato { get; set; }
        public string DataInizio { get; set; }
        public string DataFine { get; set; }
        private Database DB = new Database("127.0.0.1", "todoprova");
        private const string T = "tab_attivita";
        private const string cID = "id";
        private const string cNOME = "nome";
        private const string cSTATO = "stato";
        private const string cINIZIO = "inizio";
        private const string cFINE = "fine";

        public bool LoadFromDb()
        {
            bool risposta = true;
            string sql = $"SELECT * FROM `{T}`";
            MySqlDataReader result;
            try
            {
                DB.Open();
                if(Id > 0){
                    sql += ($" WHERE `{cID}` = {Convert.ToString(Id)};");
                }
                result = DB.SendQuery(sql);
                if(result.HasRows){
                    while(result.Read()){
                        Id = Convert.ToInt64(result[cID]);
                        Name = Convert.ToString(result[cNOME]);
                        Stato = Convert.ToString(result[cSTATO]);
                        DataInizio = Convert.ToString(result[cINIZIO]);
                        DataFine = Convert.ToString(result[cFINE]);
                    }
                }else{
                    risposta = false;
                }
                DB.Close();
            }
            catch (MySqlException)
            {
                risposta = false;
            }
            
            return risposta;
        }

        public bool SaveToDb()
        {
            bool risposta = true;
            try
            {
                DB.Open();
                if(Id == 0){
                    string sql = $"INSERT INTO `{T}` (`{cID}`, `{cNOME}`, `{cSTATO}`, `{cINIZIO}`, `{cFINE}`)";
                    sql += $" VALUES (NULL, '{Name}', {Stato}, '{DataInizio}', '{DataFine}');";
                    DB.SendNonQuery(sql);
                }
                if(Id > 0){
                    string sql = $"UPDATE `{T}`SET ";
                    sql += $"`{cNOME}` = '{Name}', ";
                    sql += $"`{cSTATO}` = {Stato}, ";
                    sql += $"`{cINIZIO}` = '{DataInizio}', ";
                    sql += $"`{cFINE}` = '{DataFine}' ";
                    sql += $"WHERE `{T}`.`{cID}` = {Id};";
                    DB.SendNonQuery(sql);
                }
                DB.Close();
            }
            catch (MySqlException)
            {
                risposta = false;
            }

            return risposta;
        }

        public bool DeleteFromDb()
        {
            bool risposta = true;
            try
            {
                DB.Open();
                if(Id > 0){
                    DB.SendNonQuery($"DELETE FROM `{T}` WHERE `{T}`.`{cID}` = {Id}");
                }else{
                    risposta = false;
                }
                DB.Close();
            }
            catch (MySqlException)
            {
                risposta = false;
            }

            return risposta;
        }

        /*prende tutti i record dalla tabella*/
        public static List<TodoItem> SelectAllRecords()
        {
            List<TodoItem> queryResult = new List<TodoItem>();
            Database DB = new Database("127.0.0.1", "todoprova");
            DB.Open();
            MySqlDataReader result = DB.SendQuery($"SELECT * FROM `{T}`;"); 

            while(result.Read()){
                queryResult.Add(new TodoItem{
                    Id = Convert.ToInt64(result[cID]),
                    Name = Convert.ToString(result[cNOME]),
                    Stato = Convert.ToString(result[cSTATO]),
                    DataInizio = Convert.ToString(result[cINIZIO]),
                    DataFine = Convert.ToString(result[cFINE])
                });
            }
            DB.Close();
            DB = null;
            
            return queryResult; 
        }

        public override string ToString() => $"{Id} - {Name} - {Stato} - {DataInizio} - {DataFine}";
    }
}
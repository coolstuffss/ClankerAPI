using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ClankerAPI.Models
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
        private const string CId = "id";
        private const string CNome = "nome";
        private const string CStato = "stato";
        private const string CInizio = "inizio";
        private const string CFine = "fine";

        public bool LoadFromDb()
        {
            bool risposta = true;
            string sql = "";
            MySqlDataReader result;
            try
            {
                DB.Open();
                if (Id == 0) {
                    sql = $"SELECT * FROM `{T}` WHERE id = (SELECT MAX(`{CId}`) from `{T}`);";
                    result = DB.SendQuery(sql);
                    if (result.HasRows){
                        while (result.Read()){
                            Id = Convert.ToInt64(result[CId]);
                            Name = Convert.ToString(result[CNome]);
                            Stato = Convert.ToString(result[CStato]);
                            DataInizio = Convert.ToString(result[CInizio]);
                            DataFine = Convert.ToString(result[CFine]);
                        }
                    }else{
                        risposta = false;
                    }
                }
                if(Id > 0){
                    sql = $"SELECT * FROM `{T}` WHERE `{CId}` = {Convert.ToString(Id)};";
                    result = DB.SendQuery(sql);
                    if(result.HasRows){
                        while(result.Read()){
                            Id = Convert.ToInt64(result[CId]);
                            Name = Convert.ToString(result[CNome]);
                            Stato = Convert.ToString(result[CStato]);
                            DataInizio = Convert.ToString(result[CInizio]);
                            DataFine = Convert.ToString(result[CFine]);
                        }
                    }else{
                        risposta = false;
                    }
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
                    string sql = $"INSERT INTO `{T}` (`{CId}`, `{CNome}`, `{CStato}`, `{CInizio}`, `{CFine}`)";
                    sql += $" VALUES (NULL, '{Name}', {Stato}, '{DataInizio}', '{DataFine}');";
                    DB.SendNonQuery(sql);
                }
                if(Id > 0){
                    string sql = $"UPDATE `{T}`SET ";
                    sql += $"`{CNome}` = '{Name}', ";
                    sql += $"`{CStato}` = {Stato}, ";
                    sql += $"`{CInizio}` = '{DataInizio}', ";
                    sql += $"`{CFine}` = '{DataFine}' ";
                    sql += $"WHERE `{T}`.`{CId}` = {Id};";
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
                    DB.SendNonQuery($"DELETE FROM `{T}` WHERE `{T}`.`{CId}` = {Id}");
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
                    Id = Convert.ToInt64(result[CId]),
                    Name = Convert.ToString(result[CNome]),
                    Stato = Convert.ToString(result[CStato]),
                    DataInizio = Convert.ToString(result[CInizio]),
                    DataFine = Convert.ToString(result[CFine])
                });
            }
            DB.Close();
            
            return queryResult; 
        }

        public override string ToString() => $"{Id} - {Name} - {Stato} - {DataInizio} - {DataFine}";
    }
}
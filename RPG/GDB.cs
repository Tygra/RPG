using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

namespace RPG
{
    class GDB
    {
        public static IDbConnection db;

        public void InitQuestDB(IDbConnection db)
        {            
            switch (TShock.Config.StorageType.ToLower())
            {
                case "mysql":
                    string[] host = TShock.Config.MySqlHost.Split(':');
                    db = new MySqlConnection()
                    {
                        ConnectionString = string.Format("Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4}",
                        host[0],
                        host.Length == 1 ? "3306" : host[1],
                        TShock.Config.MySqlDbName,
                        TShock.Config.MySqlUsername,
                        TShock.Config.MySqlPassword)
                    };
                    break;

                case "sqlite":                    
                    string sql = Path.Combine(TShock.SavePath, "quests.sqlite");
                    db = new SqliteConnection(string.Format("uri=file://{0}.Version=3", sql));
                    break;
            }
            SqlTableCreator sqlcreator = new SqlTableCreator(db, db.GetSqlType() == SqlType.Sqlite ? (IQueryBuilder)new SqliteQueryCreator() : new MysqlQueryCreator());
            sqlcreator.EnsureTableStructure(new SqlTable("RQuests",
                new SqlColumn("ID", MySqlDbType.Int32) { Unique = true, Primary = true, AutoIncrement = true },
                new SqlColumn("User", MySqlDbType.VarChar) { Length = 30 },
                new SqlColumn("QuestID", MySqlDbType.Int32),
                new SqlColumn("Date", MySqlDbType.Text),
                new SqlColumn("Expiration", MySqlDbType.Text)
                ));
            sqlcreator.EnsureTableStructure(new SqlTable("NonRQuests",
                new SqlColumn("ID", MySqlDbType.Int32) { Unique = true, Primary = true, AutoIncrement = true },
                new SqlColumn("User", MySqlDbType.VarChar) { Length = 30 },
                new SqlColumn("QuestID", MySqlDbType.Int32),
                new SqlColumn("Date", MySqlDbType.Text)
                ));
            sqlcreator.EnsureTableStructure(new SqlTable("Adventures",
                new SqlColumn("ID", MySqlDbType.Int32) { Unique = true, Primary = true, AutoIncrement = true },
                new SqlColumn("User", MySqlDbType.VarChar) { Length = 30 },
                new SqlColumn("AdventureID", MySqlDbType.Int32),
                new SqlColumn("Date", MySqlDbType.Text),
                new SqlColumn("Expiration", MySqlDbType.Text)
                ));
            sqlcreator.EnsureTableStructure(new SqlTable("ItemLevel",
                new SqlColumn("ID", MySqlDbType.Int32) { Unique = true, Primary = true, AutoIncrement = true },
                new SqlColumn("ItemName", MySqlDbType.Text) { Length = 30 },
                new SqlColumn("Restriction", MySqlDbType.Text)
                ));
        }
    }
}

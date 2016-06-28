using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using TShockAPI.DB;
using TShockAPI;

namespace RPG
{
    public static class DBManager
    {        
        public static void AddItemEntry(string ItemName, string Restriction)
        {
            GeldarRPG.db.Query("INSERT INTO `ItemLevel` (`ItemName`,`Restriction`) VALUES (@0, @1)", ItemName, Restriction);
        }

        public static void DelItemEntry(string ItemName)
        {
            GeldarRPG.db.Query("DELETE FROM `ItemLevel` WHERE `ItemName` =@0", ItemName);
        }
    }
}

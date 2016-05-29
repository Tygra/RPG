using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TShockAPI.DB;
using TShockAPI;

namespace RPG
{
    public static class DBManager
    {
        public static void AddItemEntry(DBInfo info)
        {
            GDB.db.Query("INSERT INTO `ItemLevel` (`ItemName`,`Restriction`) VALUES (@0, @1)", new object[]
            {
                info.ItemName,
                info.Restriction
            });
        }

        public static void DelItemEntry(DBInfo info)
        {
            GDB.db.Query("DELETE FROM `ItemLevel` WHERE `ItemName` =@0", new object[]
            {
                info.ItemName
            });
        }
    }
}

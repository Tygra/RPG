using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using TShockAPI;

namespace RPG
{
    public struct DBInfo
    {
        public string ItemName;
        public string Restriction;

        public DBInfo(string itemname, string restriction)
        {
            this.ItemName = itemname;
            this.Restriction = restriction;
        }
    }
}

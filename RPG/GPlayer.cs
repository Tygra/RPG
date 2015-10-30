using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using TShockAPI;

namespace RPG
{
    public class GPlayer
    {
        public int Index { get; set; }
        public TSPlayer TSPlayer { get { return TShock.Players[Index]; } }
        public int pyramid1cd { get; set; }

        public GPlayer(int index)
        {
            this.Index = index;
        }
    }
}

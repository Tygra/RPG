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
        public int pyramid2cd { get; set; }
        public int pyramid3cd { get; set; }
        public int pyramid4cd { get; set; }
        public int pyramid5cd { get; set; }
        public int pyramid6cd { get; set; }
        public int pyramid7cd { get; set; }
        public int pyramid8cd { get; set; }
        public int ice1cd { get; set; }
        public int ice2cd { get; set; }
        public int ice3cd { get; set; }
        public int ice4cd { get; set; }
        public int ice5cd { get; set; }
        public int ice6cd { get; set; }

        public GPlayer(int index)
        {
            this.Index = index;
        }
    }
}

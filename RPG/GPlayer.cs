/*  
 *  The plugin has some features which I got from other authors.
 *  I don't claim any overship over those elements which were made by someone else.
 *  The plugin has been customized to fit our need at Geldar,
 *  and because of this, it's useless for anyone else.
 *  I know timers are shit, and If someone knows a way to keep them after relog, tell me.
*/

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
        public int corr1cd { get; set; }
        public int corr2cd { get; set; }
        public int corr3cd { get; set; }
        public int corr4cd { get; set; }
        public int crim1cd { get; set; }
        public int crim2cd { get; set; }
        public int crim3cd { get; set; }
        public int crim4cd { get; set; }
        public int jadv1cd { get; set; }
        public int jadv2cd { get; set; }
        public int jadv3cd { get; set; }
        public int jadv4cd { get; set; }
        public int jadv5cd { get; set; }
        public int space1cd { get; set; }
        public int space2cd { get; set; }
        public int space3cd { get; set; }
        public int space4cd { get; set; }
        public int hallow1cd { get; set; }
        public int hallow2cd { get; set; }
        public int hallow3cd { get; set; }
        public int hallow4cd { get; set; }
        public int hallow5cd { get; set; }
        public int girocd { get; set; }
        public int greekonecd { get; set; }
        public int cavecd { get; set; }
        public int lab1cd { get; set; }
        public int lab2cd { get; set; }
        public int hiddencd { get; set; }
        public int millcd { get; set; }
        public int facepalmcd { get; set; }
        public int slapallcd { get; set; }
        public int giftcd { get; set; }
        public int mgcd { get; set; }
        public int qlab1cd { get; set; }
        public int qlab2cd { get; set; }
        public int qlab3cd { get; set; }
        public int shrine1cd { get; set; }
        public int shrine2cd { get; set; }
        public int shrine3cd { get; set; }
        public int shrine4cd { get; set; }
        public int shrine5cd { get; set; }
        public int vikingcd { get; set; }
        public int vulcancd { get; set; }
        public int startercd { get; set; }
        public int overgrowncd { get; set; }
        public int dungeoncd { get; set; }
        public int frozencd { get; set; }
        public int corruptedcd { get; set; }        
        public int hivecd { get; set; }
        public int highlandercd { get; set; }        

        public GPlayer(int index)
        {
            this.Index = index;
        }
    }
}

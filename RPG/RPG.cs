﻿using System;
using System.IO;
using System.ComponentModel;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using Wolfje.Plugins.SEconomy;
using Wolfje.Plugins.SEconomy.Journal;
using Newtonsoft.Json;

namespace RPG
{
    [ApiVersion(1, 22)]
    public class RPG : TerrariaPlugin
    {
        public DateTime LastCheck = DateTime.UtcNow;
        public DateTime SLastCheck = DateTime.UtcNow;
        public GPlayer[] Playerlist = new GPlayer[256];
        DateTime DLastCheck = DateTime.UtcNow;
        public TShockAPI.DB.Region Region { get; set; }
        public override string Name
        {
            get
            {
                return "RPG Commands";
            }
        }
        public override string Author
        {
            get
            {
                return "Tygra";
            }
        }
        public override string Description
        {
            get
            {
                return "Geldar RPG Commands";
            }
        }
        public override Version Version
        {
            get
            {
                return new Version("1.0");
            }
        }
        public RPG(Main game)
            : base(game)
        {
            Order = 1;
        }

        #region Initialize
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("geldar.admin", Reloadcfg, "rpgreload"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Adv, "adventure"));
            //Commands.ChatCommands.Add(new Command("geldar.admin", qmode, "questmode"));
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            ServerApi.Hooks.GameUpdate.Register(this, Cooldowns);
           // Commands.ChatCommands.Add(new Command(ilevel, "ilevel", "il"));
           // Commands.ChatCommands.Add(new Command(clevel, "clevel", "cl"));
            if (!Config.ReadConfig())
            {
                TShock.Log.ConsoleError("Delete config because it failed to load.");
            }
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameUpdate.Deregister(this, Cooldowns);
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
            }

        }
        #endregion

        #region Playerlist Join/Leave
        public void OnJoin(JoinEventArgs args)
        {
            Playerlist[args.Who] = new GPlayer(args.Who);
        }

        public void OnLeave(LeaveEventArgs args)
        {
            Playerlist[args.Who] = null;
        }
        #endregion

        #region Cooldown
        private void Cooldowns(EventArgs args)
        {
            if ((DateTime.UtcNow - LastCheck).TotalSeconds >= 1)
            {
                LastCheck = DateTime.UtcNow;
                foreach ( var player in Playerlist)
                {
                    if (player == null)
                    {
                        continue;
                    }
                    if (player.pyramid1cd > 0)
                    {
                        player.pyramid1cd--;
                    }
                }
            }
        }
        #endregion
        private void Adv(CommandArgs args)
        {
            if(args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Use the command below.", Color.Goldenrod);
                args.Player.SendMessage("Info: /adventure teleport - Teleports you to the adventure tower.", Color.SkyBlue);
                args.Player.SendMessage("Info: Adventure subcommands can be found in each adventure zone.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                case "teleport":
                    {
                        TShockAPI.Commands.HandleCommand(TSPlayer.Server,"/sudo -f " + args.Player.Name + " /tppos 1241 555");
                    }
                    break;

                case "pyramid1":
                    {
                        var player = Playerlist[args.Player.Index];
                        if (player.pyramid1cd == 0)
                        {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid1region);
                                if (args.Player.CurrentRegion != region)
                                {

                                    TShockAPI.Commands.HandleCommand(TSPlayer.Server, "/sudo -f " + args.Player.Name + " /item 327 1");
                                    args.Player.SendMessage("You just looted some shit", Color.Goldenrod);
                                    if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                    {
                                        player.pyramid1cd = Config.contents.pyramid1cd;
                                    }
                                    
                                }
                                args.Player.SendErrorMessage("You are not in the right region. You should be in hell, where you belong.");
                                return;
                        }
                        else
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid1cd));
                        }

                    }
                    break;
            }
        }
        #region Config reload

        private void Reloadcfg(CommandArgs args)
        {
            if (Config.ReadConfig())
            {
                args.Player.SendMessage("RPG config reloaded.", Color.Goldenrod);
            }

            else
            {
                args.Player.SendErrorMessage("Nope. Check logs.");
            }
        }

        #endregion
    }
}

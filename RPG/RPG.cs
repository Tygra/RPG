#region Disclaimer
/*  
 *  The plugin has some features that I got from other authors.
 *  I don't claim any ownership over those elements which were made by someone else.
 *  The plugin has been customized to fit our need at Geldar,
 *  and because of this, it's useless for anyone else.
 *  I know timers are shit, and If someone knows a way to keep them after relog, tell me.
*/
#endregion

#region Refs
using System;
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
#endregion

namespace RPG
{
    /*
     * House for tc - replicate
     * House upgrade list
     * Vip trial
     * Test mimic spawn
    */
    [ApiVersion(1, 22)]
    public class RPG : TerrariaPlugin
    {
        #region Info & other things
        public DateTime LastCheck = DateTime.UtcNow;
        public DateTime SLastCheck = DateTime.UtcNow;
        public GPlayer[] Playerlist = new GPlayer[256];
        DateTime DLastCheck = DateTime.UtcNow;
        public TShockAPI.DB.Region Region { get; set; }
        public override string Name
        { get { return "RPG Commands"; } }
        public override string Author
        { get { return "Tygra and Idris"; } }
        public override string Description
        { get { return "Geldar RPG Commads"; } }
        public override Version Version
        { get { return new Version(1, 3); } }

        public RPG(Main game)
            : base(game)
        {
            Order = 1;
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("geldar.admin", Reloadcfg, "rpgreload"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Minigame, "minigame"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Adv, "adventure"));
            Commands.ChatCommands.Add(new Command(Teleport, "teleport"));
            Commands.ChatCommands.Add(new Command("geldar.level5", Story, "story"));
            Commands.ChatCommands.Add(new Command(Tutorial, "tutorial"));
            Commands.ChatCommands.Add(new Command(Quests, "quest"));
            Commands.ChatCommands.Add(new Command(Starter, "starter"));
            Commands.ChatCommands.Add(new Command("geldar.trial", Trial, "trial"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Exban, "exban"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Exui, "exui"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Clearall, "ca"));
            Commands.ChatCommands.Add(new Command("geldar.admin", ResetWorldStats, "resetworldstats"));
            Commands.ChatCommands.Add(new Command("geldar.admin", Townnpc, "townnpc"));
            Commands.ChatCommands.Add(new Command(staff, "staff"));
            Commands.ChatCommands.Add(new Command("geldar.level30", Facepalm, "facepalm"));
            Commands.ChatCommands.Add(new Command("geldar.mod", Slapall, "slapall"));
            Commands.ChatCommands.Add(new Command("geldar.admin", Gift, "gift"));
            Commands.ChatCommands.Add(new Command("geldar.champion", Bunny, "bunny"));
            Commands.ChatCommands.Add(new Command("seconomy.world.mobgains", BankBal, "bb"));
            Commands.ChatCommands.Add(new Command("geldar.level30", MonsterGamble, "monstergamble", "mg"));
            Commands.ChatCommands.Add(new Command("geldar.vip", VIP, "vip"));
            Commands.ChatCommands.Add(new Command(Buffme, "buffme"));
            Commands.ChatCommands.Add(new Command(Geldar, "geldar"));
            //Commands.ChatCommands.Add(new Command("geldar.level30", Mimic, "mimic"));
            Commands.ChatCommands.Add(new Command("seconomy.world.mobgains", Stuck, "stuck"));
            Commands.ChatCommands.Add(new Command("geldar.level20", Housing, "housing"));
            Commands.ChatCommands.Add(new Command(checkCooldowns, "checkcd"));
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            ServerApi.Hooks.GameUpdate.Register(this, Cooldowns);
            if (!Config.ReadConfig())
            {
                TShock.Log.ConsoleError("Config loading failed. Consider deleting it.");
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
                foreach (var player in Playerlist)
                {
                    if (player == null || player.TSPlayer == null)
                    {
                        continue;
                    }
                    /*
                    int[] _cdlist =
                    {
                        player.pyramid1cd,player.pyramid2cd,player.pyramid3cd,player.pyramid4cd,player.pyramid5cd,player.pyramid6cd,player.pyramid7cd,player.pyramid8cd,
                        player.ice1cd,player.ice2cd,player.ice3cd,player.ice4cd,player.ice5cd,player.ice6cd,
                        player.corr1cd,player.corr2cd,player.corr3cd,player.corr4cd,
                        player.crim1cd,player.crim2cd,player.crim3cd,player.crim4cd,
                        player.jadv1cd,player.jadv2cd,player.jadv3cd,player.jadv4cd,player.jadv5cd,
                        player.space1cd,player.space2cd,player.space3cd,player.space4cd,
                        player.hallow1cd,player.hallow2cd,player.hallow3cd,player.hallow4cd,player.hallow5cd,
                        player.girocd,
                        player.greekonecd,
                        player.cavecd,
                        player.lab1cd,player.lab2cd,
                        player.hiddencd,
                        player.facepalmcd,
                        player.slapallcd,
                        player.giftcd,
                        player.mgcd,
                        player.qlab1cd,player.qlab2cd,player.qlab3cd,
                        player.shrine1cd,player.shrine2cd,player.shrine3cd,player.shrine4cd,player.shrine5cd,
                        player.vikingcd,
                        player.vulcancd,
                        player.startercd,
                        player.dungeoncd,
                        player.corruptedcd,
                        player.overgrowncd,
                        player.frozencd,
                        player.hivecd,
                        player.highlandercd,
                        player.hellq1cd,player.hellq2cd,
                        player.hunter1cd,player.hunter2cd,
                        player.buff1cd,player.buff2cd,player.buff3cd,player.buff4cd
                    };

                    for (int i = 0; i < _cdlist.Length; i++)
                    {
                        if (_cdlist[i] > 0) _cdlist[i]--;
                        TShock.Log.ConsoleError(i.ToString());
                    }*/
                    TShock.Log.ConsoleError(player.pyramid1cd.ToString());
                    /* changed the massive piles of if statements to a more simpler way. */
                    if (player.pyramid1cd > 0) player.pyramid1cd--;
                    if (player.pyramid2cd > 0) player.pyramid2cd--;
                    if (player.pyramid3cd > 0) player.pyramid3cd--;
                    if (player.pyramid4cd > 0) player.pyramid4cd--;
                    if (player.pyramid5cd > 0) player.pyramid5cd--;
                    if (player.pyramid6cd > 0) player.pyramid6cd--;
                    if (player.pyramid7cd > 0) player.pyramid7cd--;
                    if (player.pyramid8cd > 0) player.pyramid8cd--;
                    if (player.ice1cd > 0) player.ice1cd--;
                    if (player.ice2cd > 0) player.ice2cd--;
                    if (player.ice3cd > 0) player.ice3cd--;
                    if (player.ice4cd > 0) player.ice4cd--;
                    if (player.ice5cd > 0) player.ice5cd--;
                    if (player.ice6cd > 0) player.ice6cd--;
                    if (player.corr1cd > 0) player.corr1cd--;
                    if (player.corr2cd > 0) player.corr2cd--;
                    if (player.corr3cd > 0) player.corr3cd--;
                    if (player.corr4cd > 0) player.corr4cd--;
                    if (player.crim1cd > 0) player.crim1cd--;
                    if (player.crim2cd > 0) player.crim2cd--;
                    if (player.crim3cd > 0) player.crim3cd--;
                    if (player.crim4cd > 0) player.crim4cd--;
                    if (player.jadv1cd > 0) player.jadv1cd--;
                    if (player.jadv2cd > 0) player.jadv2cd--;
                    if (player.jadv3cd > 0) player.jadv3cd--;
                    if (player.jadv4cd > 0) player.jadv4cd--;
                    if (player.jadv5cd > 0) player.jadv5cd--;
                    if (player.space1cd > 0) player.space1cd--;
                    if (player.space2cd > 0) player.space2cd--;
                    if (player.space3cd > 0) player.space3cd--;
                    if (player.space4cd > 0) player.space4cd--;
                    if (player.hallow1cd > 0) player.hallow1cd--;
                    if (player.hallow2cd > 0) player.hallow2cd--;
                    if (player.hallow3cd > 0) player.hallow3cd--;
                    if (player.hallow4cd > 0) player.hallow4cd--;
                    if (player.hallow5cd > 0) player.hallow5cd--;
                    if (player.girocd > 0) player.girocd--;
                    if (player.greekonecd > 0) player.greekonecd--;
                    if (player.cavecd > 0) player.cavecd--;
                    if (player.lab1cd > 0) player.lab1cd--;
                    if (player.lab2cd > 0) player.lab2cd--;
                    if (player.hiddencd > 0) player.hiddencd--;
                    if (player.facepalmcd > 0)player.facepalmcd--;
                    if (player.slapallcd > 0)player.slapallcd--;
                    if (player.giftcd > 0)player.giftcd--;
                    if (player.mgcd > 0) player.mgcd--;
                    if (player.qlab1cd > 0) player.qlab1cd--;
                    if (player.qlab2cd > 0) player.qlab2cd--;
                    if (player.qlab3cd > 0) player.qlab3cd--;
                    if (player.shrine1cd > 0) player.shrine1cd--;
                    if (player.shrine2cd > 0) player.shrine2cd--;
                    if (player.shrine3cd > 0) player.shrine3cd--;
                    if (player.shrine4cd > 0) player.shrine4cd--;
                    if (player.shrine5cd > 0) player.shrine5cd--;
                    if (player.vikingcd > 0) player.vikingcd--;
                    if (player.vulcancd > 0) player.vulcancd--;
                    if (player.startercd > 0) player.startercd--;
                    if (player.dungeoncd > 0) player.dungeoncd--;
                    if (player.corruptedcd > 0) player.corruptedcd--;
                    if (player.overgrowncd > 0) player.overgrowncd--;
                    if (player.frozencd > 0) player.frozencd--;
                    if (player.hivecd > 0) player.hivecd--;
                    if (player.highlandercd > 0) player.highlandercd--;
                    if (player.hellq1cd > 0) player.hellq1cd--;
                    if (player.hellq2cd > 0) player.hellq2cd--;
                    if (player.hunter1cd > 0) player.hunter1cd--;
                    if (player.hunter2cd > 0) player.hunter2cd--;
                    if (player.buff1cd > 0) player.buff1cd--;
                    if (player.buff2cd > 0) player.buff2cd--;                      
                    if (player.buff3cd > 0) player.buff3cd--;
                    if (player.buff4cd > 0) player.buff4cd--;
                }
            }
        }
        #endregion Cooldown

        #region Staffcommands

        #region Extended ban
        private void Exban(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax: /exban \"<player name>\"");
            }
            else
            {
                string text = args.Parameters[0];
                Ban banByName = TShock.Bans.GetBanByName(text, true);
                if (banByName == null)
                {
                    args.Player.SendErrorMessage("No bans by this name were found.");
                }
                else
                {
                    args.Player.SendInfoMessage(string.Concat(new string[] { "Account name: ", banByName.Name, " (", banByName.IP, ")" }));
                    args.Player.SendInfoMessage("Date banned: " + banByName.Date);
                    if (banByName.Expiration != "")
                    {
                        args.Player.SendInfoMessage("Expiration date: " + banByName.Expiration);
                    }
                    args.Player.SendInfoMessage("Banned by: " + banByName.BanningUser);
                    args.Player.SendInfoMessage("Reason: " + banByName.Reason);
                }
            }
        }
        #endregion

        #region Exui
        private void Exui(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {
                string text = string.Join(" ", args.Parameters);
                if (text != null & text != "")
                {
                    User userByName = TShock.Users.GetUserByName(text);
                    if (userByName != null)
                    {
                        args.Player.SendMessage("Query result: ", Color.Goldenrod);
                        args.Player.SendMessage(string.Format("User {0} exists.", text), Color.SkyBlue);
                        try
                        {
                            DateTime dateTime = DateTime.Parse(userByName.Registered);
                            DateTime dateTime2 = DateTime.Parse(userByName.LastAccessed);
                            List<string> list = JsonConvert.DeserializeObject<List<string>>(userByName.KnownIps);
                            string arg = list[list.Count - 1];
                            args.Player.SendMessage(string.Format("{0}'s group is {1}.", text, userByName.Group), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0}'s last known IP is {1}.", text, arg), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} registered on {1}.", text, dateTime.ToShortDateString()), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} was last seen {1}.", text, dateTime2.ToShortDateString(), dateTime2.ToShortTimeString()), Color.SkyBlue);
                        }
                        catch
                        {
                            DateTime dateTime = DateTime.Parse(userByName.Registered);
                            args.Player.SendMessage(string.Format("{0}'s group is {1}.", text, userByName.Group), Color.SkyBlue);
                            args.Player.SendMessage(string.Format("{0} registered on {1}.", text, dateTime.ToShortDateString()), Color.SkyBlue);
                        }
                    }
                    else
                    {
                        args.Player.SendMessage(string.Format("User {0} does not exist.", text), Color.Red);
                    }
                }
                else
                {
                    args.Player.SendErrorMessage("Syntax: /exui \"<player name>\"");
                }
            }
            else
            {
                args.Player.SendErrorMessage("Syntax: /exui \"<player name>\"");
            }
        }
        #endregion

        #region Clearall
        private void Clearall(CommandArgs args)
        {
            TShockAPI.Commands.HandleCommand(args.Player, "/clear item 9000");
            TShockAPI.Commands.HandleCommand(args.Player, "/clear projectile 9000");
        }
        #endregion

        #region ResetWorldStats

        public static void ResetWorldStats(CommandArgs args)
        {
            NPC.downedBoss1 = false;
            NPC.downedBoss2 = false;
            NPC.downedBoss3 = false;
            NPC.downedQueenBee = false;
            NPC.downedSlimeKing = false;
            NPC.downedMechBossAny = false;
            NPC.downedMechBoss1 = false;
            NPC.downedMechBoss2 = false;
            NPC.downedMechBoss3 = false;
            NPC.downedFishron = false;
            NPC.downedMartians = false;
            NPC.downedAncientCultist = false;
            NPC.downedMoonlord = false;
            NPC.downedHalloweenKing = false;
            NPC.downedHalloweenTree = false;
            NPC.downedChristmasIceQueen = false;
            NPC.downedChristmasSantank = false;
            NPC.downedChristmasTree = false;
            NPC.downedPlantBoss = false;
            NPC.downedGoblins = false;
            NPC.downedClown = false;
            NPC.downedFrost = false;
            NPC.downedPirates = false;
            NPC.savedAngler = false;
            NPC.downedGolemBoss = false;
            WorldGen.shadowOrbSmashed = false;
            WorldGen.altarCount = 0;
            WorldGen.shadowOrbCount = 0;
            args.Player.SendSuccessMessage("The World Generation stats have been reset.");
        }

        #endregion

        #region Townnpc
        private void Townnpc(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Spawn town npcs to pre-define position.", Color.Goldenrod);
                args.Player.SendMessage("/townnpc witch - Spawns the Withc Doctor in the jungle.", Color.Goldenrod);
                args.Player.SendMessage("If it's needed then I'll add more of these.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                case "witch":
                    {
                        TShockAPI.Commands.HandleCommand(TSPlayer.Server, " /spawnmob 228 1 2585 599");
                        args.Player.SendSuccessMessage("Do ya feel da voodo?");
                    }
                    break;
            }
        }
        #endregion

        #endregion

        #region Other commands

        #region Staff
        public void staff(CommandArgs args)
        {
            List<TSPlayer> list = new List<TSPlayer>(TShock.Players).FindAll((TSPlayer t) => t != null && t.Group.HasPermission("geldar.mod"));
            if (list.Count == 0)
            {
                args.Player.SendErrorMessage("No staff members currently online. If you have a problem check our website at www.geldar.net.");
            }
            else
            {
                args.Player.SendMessage("[Currently online staff members]", Color.Goldenrod);
                foreach (TSPlayer current in list)
                {
                    if (current != null)
                    {
                        Color color = new Color((int)current.Group.R, (int)current.Group.G, (int)current.Group.B);
                        args.Player.SendMessage(string.Format("{0}{1}", current.Group.Prefix, current.Name), color);
                    }
                }
            }
        }
        #endregion

        #region BankBal
        private void BankBal(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/bank bal");
        }
        #endregion

        #region Facepalm
        private void Facepalm(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            if (player.facepalmcd != 0)
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.facepalmcd));
                args.Player.SendErrorMessage("Chill, facepalming repeatedly can be fatal.");
                return;
            }
            else
            {
                TSPlayer.All.SendMessage(string.Format("{0} facepalmed.", args.Player.Name), Color.Goldenrod);
                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                {
                    player.facepalmcd = Config.contents.facepalmcd;
                }
            }
        }
        #endregion

        #region Slapall
        private void Slapall(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            if (player.slapallcd != 0)
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.slapallcd));
                args.Player.SendErrorMessage("The slapdemic has to wait");
                return;
            }
            else
            {
                TSPlayer.All.DamagePlayer(1);
                TSPlayer.All.SendMessage(string.Format("{0} slapped everyone.", args.Player.Name), Color.Goldenrod);
                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                {
                    player.slapallcd = Config.contents.slapallcd;
                }
            }
        }
        #endregion

        #region Gift
        private void Gift(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            Region region = TShock.Regions.GetRegionByName(Config.contents.giftregion);
            if (player.giftcd != 0)
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.giftcd));
                return;
            }
            if (args.Player.CurrentRegion != region)
            {
                args.Player.SendErrorMessage("You are not in the right region. Requirement: Big Christmas Tree.");
                args.Player.SendErrorMessage("This command will only be available while Terraria is in the Winter Holiday mode.");
                return;
            }
            if (args.Player.CurrentRegion == null)
            {
                args.Player.SendErrorMessage("You are not in the right region. Requirement: Big Christmas Tree.");
                args.Player.SendErrorMessage("This command will only be available while Terraria is in the Winter Holiday mode.");
                return;
            }
            if (args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("Correct syntax: /gift \"player name\"");
                return;
            }
            else
            {
                List<TSPlayer> list = TShock.Utils.FindPlayer(args.Parameters[0]);
                if (list.Count == 0)
                {
                    args.Player.SendErrorMessage("No player found by that name.");
                }
                else if (list.Count > 1)
                {
                    TShock.Utils.SendMultipleMatchError(args.Player, from p in list select p.Name);
                }
                else
                {
                    TSPlayer tSPlayer = list[0];
                    Random random = new Random();
                    if (random.Next(2) == 0)
                    {
                        Item itemById = TShock.Utils.GetItemById(Config.contents.giftitem1);
                        tSPlayer.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, itemById.maxStack, 0);
                        tSPlayer.SendInfoMessage("{0} gave you Coal. Maybe you did something bad.", args.Player.Name, Color.Goldenrod);
                        args.Player.SendSuccessMessage("You gave {0} Coal. Maybe {0} did something bad.", tSPlayer.Name, Color.Goldenrod);
                    }
                    else
                    {
                        Item itemById = TShock.Utils.GetItemById(Config.contents.giftitem2);
                        tSPlayer.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, itemById.stack, 0);
                        tSPlayer.SendInfoMessage("{0} gave you a Present. Merry Christmas!", args.Player.Name, Color.Goldenrod);
                        args.Player.SendSuccessMessage("You gave {0} a Present.", tSPlayer.Name, Color.Goldenrod);
                        if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                        {
                            player.giftcd = Config.contents.giftcd;
                        }
                    }
                }
            }
        }
        #endregion

        #region Bunny
        private void Bunny(CommandArgs args)
        {
            args.Player.SendMessage("Don't feed the rabbit after midnight.", Color.Goldenrod);
            args.Player.SetBuff(40, 60, true);
        }
        #endregion

        #region MonsterGamble
        private void MonsterGamble(CommandArgs args)
        {
            Random random = new Random();
            int amount = random.Next(1, 30);
            var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
            var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
            var playeramount = selectedPlayer.Balance;
            var player = Playerlist[args.Player.Index];
            Money moneyamount = -Config.contents.mgcost;
            Money moneyamount2 = Config.contents.mgcost;

            if (player.mgcd == 0)
            {
                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                {
                    player.mgcd = Config.contents.mgcd;
                }

                if (Config.contents.SEconomy)
                {
                    {
                        if (!args.Player.Group.HasPermission("geldar.nocost"))
                        {
                            if (playeramount > moneyamount2)
                            {
                                int monsteramount;
                                do
                                {
                                    monsteramount = random.Next(1, 539);
                                    args.Player.SendInfoMessage("You have gambled a banned monster, attempting to regamble...", Color.Yellow);
                                } while (Config.contents.mgexlcude.Contains(monsteramount));

                                NPC npcs = TShock.Utils.GetNPCById(monsteramount);
                                TSPlayer.Server.SpawnNPC(npcs.type, npcs.name, amount, args.Player.TileX, args.Player.TileY, 50, 20);
                                args.Player.SendSuccessMessage("You have lost {0} for monster gambling.", moneyamount2);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("{0} has been lost for monster gambling", moneyamount2, args.Player.Name), string.Format("CawAIO: " + "Monster Gambling"));
                                args.Player.SendSuccessMessage("You spawnned {0} {1}.", amount, npcs.name);
                                TShock.Log.ConsoleInfo("{0} has spawnned {1} {2}.", args.Player.Name, amount, npcs.name);
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You need {0} to gamble, you have {1}.", moneyamount2, selectedPlayer.Balance);
                            }
                        }
                        else
                        {
                            if (args.Player.Group.HasPermission("geldar.nocost"))
                            {
                                int monsteramount;
                                do
                                {
                                    monsteramount = random.Next(1, 539);
                                } while (Config.contents.mgexlcude.Contains(monsteramount));
                                NPC npcs = TShock.Utils.GetNPCById(monsteramount);
                                TSPlayer.Server.SpawnNPC(npcs.type, npcs.name, amount, args.Player.TileX, args.Player.TileY, 50, 20);
                                args.Player.SendSuccessMessage("You have lost nothing for monster gambling.");
                                args.Player.SendSuccessMessage("You spawnned {0} {1}.", amount, npcs.name);
                                TShock.Log.ConsoleInfo("{0} has spawnned {1} {2}.", args.Player.Name, amount, npcs.name);
                            }
                        }
                    }
                }
                else
                {
                    int Randnpc;

                    do Randnpc = random.Next(1, 539);
                    while (Config.contents.mgexlcude.Contains(Randnpc));

                    NPC npcs = TShock.Utils.GetNPCById(Randnpc);
                    TSPlayer.Server.SpawnNPC(npcs.type, npcs.name, amount, args.Player.TileX, args.Player.TileY, 50, 20);

                    TSPlayer.All.SendSuccessMessage(string.Format("{0} has randomly spawned {1} {2} time(s).", args.Player.Name,
                        npcs.name, amount));
                    args.Player.SendSuccessMessage("You spawnned {0} {1}.", amount, npcs.name);
                    TShock.Log.ConsoleInfo("{0} has spawnned {1} {2}.", args.Player.Name, amount, npcs.name);
                }
            }
            else
            {
                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.mgcd));
            }
        }
        #endregion

        #region Starter
        private void Starter(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: If you lost your starter weapon here you can replace it.", Color.Goldenrod);
                args.Player.SendMessage("Info: Use the command appropriate to your class /starter mage/warrior/ranger/summoner", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Starter Mage
                case "mage":
                    {
                        if (args.Player.Group.HasPermission("geldar.starter.mage"))
                        {
                            var player = Playerlist[args.Player.Index];
                            if (player.startercd != 0)
                            {
                                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.startercd));
                                return;
                            }
                            else
                            {
                                if (args.Player.InventorySlotAvailable)
                                {
                                    Item itemById = TShock.Utils.GetItemById(Config.contents.startermage);
                                    args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                    if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                    {
                                        player.startercd = Config.contents.startercd;
                                    }
                                }
                                else
                                {
                                    args.Player.SendErrorMessage("Your inventory seems to be full. Free up one slot.");
                                }

                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 10 or under, or you executed the wrong class command.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Starter Warrior
                case "warrior":
                    {
                        if (args.Player.Group.HasPermission("geldar.starter.warrior"))
                        {
                            var player = Playerlist[args.Player.Index];
                            if (player.startercd != 0)
                            {
                                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.startercd));
                                return;
                            }
                            else
                            {
                                if (args.Player.InventorySlotAvailable)
                                {
                                    Item itemById = TShock.Utils.GetItemById(Config.contents.starterwarrior);
                                    args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                    if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                    {
                                        player.startercd = Config.contents.startercd;
                                    }
                                }
                                else
                                {
                                    args.Player.SendErrorMessage("Your inventory seems to be full. Free up one slot.");
                                }

                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 10 or under, or you executed the wrong class command.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Starter Ranger
                case "ranger":
                    {
                        if (args.Player.Group.HasPermission("geldar.starter.ranger"))
                        {
                            var player = Playerlist[args.Player.Index];
                            if (player.startercd != 0)
                            {
                                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.startercd));
                                return;
                            }
                            else
                            {
                                if (args.Player.InventorySlotAvailable)
                                {
                                    Item itemById = TShock.Utils.GetItemById(Config.contents.starterranger);
                                    args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                    if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                    {
                                        player.startercd = Config.contents.startercd;
                                    }
                                }
                                else
                                {
                                    args.Player.SendErrorMessage("Your inventory seems to be full. Free up one slot.");
                                }

                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 10 or under, or you executed the wrong class command.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Starter Summoner
                case "summoner":
                    {
                        if (args.Player.Group.HasPermission("geldar.starter.summoner"))
                        {
                            var player = Playerlist[args.Player.Index];
                            if (player.startercd != 0)
                            {
                                args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.startercd));
                                return;
                            }
                            else
                            {
                                if (args.Player.InventorySlotAvailable)
                                {
                                    Item itemById = TShock.Utils.GetItemById(Config.contents.startersummoner);
                                    args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                    if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                    {
                                        player.startercd = Config.contents.startercd;
                                    }
                                }
                                else
                                {
                                    args.Player.SendErrorMessage("Your inventory seems to be full. Free up one slot.");
                                }

                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not a level 20 or under summoner, or you executed the wrong class command.");
                            return;
                        }
                    }
                    break;
                    #endregion
            }
        }
        #endregion

        #region Stuck
        private void Stuck(CommandArgs args)
        {
            TShock.Utils.Kick(args.Player, "Player got kicked to get unstuck with banned item.");
            return;
        }
        #endregion

        #region Mimic
        /*
        private void Mimic(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendInfoMessage("You can spawn the new mimics if you have the necessary items for it.");
                args.Player.SendInfoMessage("Use /mimic hallowed/crimson/corrupt.");
                args.Player.SendInfoMessage("It will only work with a wooden chest.");
                return;
            }

            switch (args.Parameters[0])
            {
                #region Hallowed
                case "hallowed":
                    {
                        var searchforkey = args.TPlayer.inventory.FirstOrDefault(i => i.netID == (int)Config.contents.mimichallowkey);
                        var searchforchest = args.TPlayer.inventory.FirstOrDefault(i => i.netID == (int)Config.contents.mimicchest);
                        if (searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have the key in your inventory.");
                            return;
                        }
                        if (searchforchest == null)
                        {
                            args.Player.SendErrorMessage("You don't have a chest in your intentory.");
                            return;
                        }
                        if (searchforchest == null && searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have any of the required items in your inventory.");
                            return;
                        }

                        Item item;
                        for (int i = 0; i < 50; i++)
                        {
                            item = args.TPlayer.inventory[i];

                            if (item.netID == (int)Config.contents.mimichallowkey)
                            {
                                if (item.stack == 1)
                                {
                                    args.TPlayer.inventory[i].stack--;
                                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, string.Empty, args.Player.Index, i);
                                    Item take = TShock.Utils.GetItemById((int)Config.contents.mimichallowkey);
                                    Item take2 = TShock.Utils.GetItemById((int)Config.contents.mimicchest);
                                    var npc = TShock.Utils.GetNPCById(Config.contents.mimichallow);
                                    TSPlayer.Server.SpawnNPC(Config.contents.mimichallow, npc.name, 1, args.Player.TileX, args.Player.TileY);
                                }   
                            }
                        }
                    }
                    break;
                #endregion

                #region Crimson
                case "crimson":
                    {
                        var searchforkey = args.TPlayer.inventory.FirstOrDefault(i => i.netID == Config.contents.mimiccrimsonkey);
                        var searchforchest = args.TPlayer.inventory.FirstOrDefault(i => i.netID == Config.contents.mimicchest);
                        if (searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have the key in your inventory.");
                            return;
                        }
                        if (searchforchest == null)
                        {
                            args.Player.SendErrorMessage("You don't have a chest in your intentory.");
                            return;
                        }
                        if (searchforchest == null || searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have one of the required items in your inventory.");
                            return;
                        }

                        Item item;
                        for (int i = 0; i < 50; i++)
                        {
                            item = args.TPlayer.inventory[i];

                            if (item.netID == Config.contents.mimiccrimsonkey && item.netID == Config.contents.mimicchest)
                            {
                                if (item.stack == 1)
                                {
                                    args.TPlayer.inventory[i].stack--;
                                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, "", args.Player.Index, i);
                                    NetMessage.SendData((int)PacketTypes.PlayerSlot, args.Player.Index, -1, "", args.Player.Index, i);
                                    Item take = TShock.Utils.GetItemById(Config.contents.mimiccrimsonkey);
                                    Item take2 = TShock.Utils.GetItemById(Config.contents.mimicchest);
                                    var npc = TShock.Utils.GetNPCById(Config.contents.mimiccrimson);
                                    TSPlayer.Server.SpawnNPC(Config.contents.mimiccrimson, npc.name, 1, args.Player.TileX, args.Player.TileY);
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region Corrupt
                case "corrupt":
                    {
                        var searchforkey = args.TPlayer.inventory.FirstOrDefault(i => i.netID == (int)Config.contents.mimiccorruptkey);
                        var searchforchest = args.TPlayer.inventory.FirstOrDefault(i => i.netID == (int)Config.contents.mimicchest);
                        if (searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have the key in your inventory.");
                            return;
                        }
                        if (searchforchest == null)
                        {
                            args.Player.SendErrorMessage("You don't have a chest in your intentory.");
                            return;
                        }
                        if (searchforchest == null && searchforkey == null)
                        {
                            args.Player.SendErrorMessage("You don't have any of the required items in your inventory.");
                            return;
                        }

                        Item item;
                        for (int i = 0; i < 50; i++)
                        {
                            item = args.TPlayer.inventory[i];

                            if (item.netID == (int)Config.contents.mimiccorruptkey)
                            {
                                if (item.stack == 1)
                                {
                                    args.TPlayer.inventory[i].stack--;
                                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, string.Empty, args.Player.Index, i);
                                    Item take = TShock.Utils.GetItemById((int)Config.contents.mimiccorruptkey);
                                    Item take2 = TShock.Utils.GetItemById((int)Config.contents.mimicchest);
                                    var npc = TShock.Utils.GetNPCById(Config.contents.mimiccorrupt);
                                    TSPlayer.Server.SpawnNPC(Config.contents.mimiccorrupt, npc.name, 1, args.Player.TileX, args.Player.TileY);
                                }
                            }
                        }
                    }
                    break;
                #endregion
            }
        }
        */
        #endregion

        #region Houseplot buying

        private void Housing(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendInfoMessage("With this command you can buy a pre-define housing plot, either on the clouds, or underground.");
                args.Player.SendInfoMessage("The plot's cost depends on where it is located. Plots closer to the teleporters are more expensive.");
                args.Player.SendInfoMessage("/housing price - will show you the price of the plot. /housing region - will show you the region's name.");
                args.Player.SendInfoMessage("Check the housing rules for restrictions. /geldar housing");
                args.Player.SendInfoMessage("With /housing alt you can add your other characters to the plot for x Terra Coins.");
                args.Player.SendInfoMessage("You need to be stading on an island for the commands to work.");
                return;
            }

            switch (args.Parameters[0])
            {
                #region Upgrade
                case "upgrade":
                    {

                    }
                    break;
                #endregion

                #region Current region
                case "region":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.abovehousing);
                        Region region2 = TShock.Regions.GetRegionByName(Config.contents.underhousing);
                        if (args.Player.CurrentRegion != region || args.Player.CurrentRegion != region2)
                        {
                            args.Player.SendErrorMessage("You are not in any housing region, go to the above or underground housing area and stand on a free spot.");
                            return;
                        }
                        else
                        {
                            args.Player.SendInfoMessage("You are in the region: {0} .", args.Player.CurrentRegion.Name);
                        }
                    }
                    break;
                #endregion

                #region Alt character support
                case "alt":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.altcost;
                        Money moneyamount2 = Config.contents.altcost;
                        Region region = args.Player.CurrentRegion;
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not standing in a housing plot.");
                            return;
                        }
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to buy this plot. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (args.Parameters.Count < 1)
                        {
                            args.Player.SendErrorMessage("Wrong username specified or you didn't put double qoutes around the name.");
                            args.Player.SendErrorMessage("Example: /housing alt \"character name\".");
                            return;
                        }
                        if (args.Player.Name != args.Player.CurrentRegion.Owner)
                        {
                            args.Player.SendErrorMessage("You are not the owner of this housing plot.");
                            return;
                        }
                        if (args.Parameters.Count == 1)
                        {
                            string altname = string.Join(" ", args.Parameters[1]);
                            if (altname != null & altname != "")
                            {
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for adding your alt character to the {1} housing plot.", moneyamount2, args.Player.CurrentRegion, args.Player.Name), string.Format("Alt adding {0} to {1}.", altname, args.Player.CurrentRegion));
                                TShock.Regions.AddNewUser(args.Player.CurrentRegion.Name, altname);
                            }
                        }

                    }
                    break;
                #endregion

                #region Housing plot 1
                case "h1":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.h1cost;
                        Money moneyamount2 = Config.contents.h1cost;
                        Region region = TShock.Regions.GetRegionByName(Config.contents.h1region);
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region for this command.");
                            args.Player.SendErrorMessage("Stand on the sign and execute the command written on the sign.");
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region for this command.");
                            args.Player.SendErrorMessage("Stand on the sign and execute the command written on the sign.");
                            return;
                        }
                        if (args.Player.Name != args.Player.CurrentRegion.Owner && args.Player.CurrentRegion.Owner != Config.contents.defaultowner)
                        {
                            args.Player.SendInfoMessage("This housing plot has already been claimed by someone.");
                            return;
                        }
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to buy this plot. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the h40 housing plot.", moneyamount2, args.Player.Name), string.Format("Housing plot {0}", args.Player.CurrentRegion));
                            TShock.Regions.ChangeOwner(Config.contents.h1region, args.Player.Name);
                            args.Player.SendInfoMessage("You bought {0}, housing plot.", args.Player.CurrentRegion);
                            return;
                        }
                    }
                    #endregion
            }
        }

        #endregion

        #region cooldown check
        
        private void checkCooldowns(CommandArgs args)
        {
            var player = Playerlist[args.Player.Index];
            if (player.pyramid1cd <= 0) player.pyramid1cd = Config.contents.pyramid1cd;
            args.Player.SendInfoMessage(player.pyramid1cd.ToString());
        }    

        #endregion

        #endregion

        #region Tutorial
        private void Tutorial(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: If you wish to be teleported to the tutorial zone, use /teleport tutorial", Color.Goldenrod);
                args.Player.SendMessage("Info: For some housing information use /tutorial housing.", Color.Goldenrod);
                args.Player.SendMessage("Info: For a trading tutorial use /tutorial trading.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region housing
                case "housing":
                    {
                        args.Player.SendMessage("------------------------ Housing Tutorial ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("You areallowed to have a house from level 20, either in the above or the underground zone.", Color.SkyBlue);
                        args.Player.SendMessage("The maximum size allowed is 15 blocks wide,12 blocks high, walls included.", Color.SkyBlue);
                        args.Player.SendMessage("Follow the guidelines at /house set 1 and /house set 2.", Color.SkyBlue);
                        args.Player.SendMessage("After you marked the spots use /house add housename. Change housename to your desired house name.", Color.SkyBlue);
                        args.Player.SendMessage("Don't forget to check the housing rules either on our website or with /geldar.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region trading
                case "trading":
                    {
                        args.Player.SendMessage("------------------------ Trading Tutorial ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("From level 5 you can sell and buy item on trade.", Color.SkyBlue);
                        args.Player.SendMessage("Example: /trade add \"cactus sword\" 1 100.", Color.SkyBlue);
                        args.Player.SendMessage("This case you are selling 1 Cactus sword for 100 Terra Coins.", Color.SkyBlue);
                        args.Player.SendMessage("If the item name is two or more word you will need quotation marks around the item name.", Color.SkyBlue);
                        args.Player.SendMessage("/trade check - to check your listed trades; /trade list - to check the full trade list", Color.SkyBlue);
                        args.Player.SendMessage("/trade collect - to collect money from finished trades; /trade cancel ID - to cancel a trade.", Color.SkyBlue);
                        args.Player.SendMessage("/trade accept ID - to buy something. You can get the ID from trade list or /trade search.", Color.SkyBlue);
                        args.Player.SendMessage("You will need qoutation marks when using /trade search \"item name\" too.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter and use the up arrow to scroll the chat upwards.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region tutclass
                case "class":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutclassregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Class tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6221 * 16, 983 * 16);
                        }
                    }
                    break;
                #endregion

                #region tutgear
                case "gear":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutgearregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Gear tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6213 * 16, 992 * 16);
                        }
                    }
                    break;
                #endregion

                #region tutmine
                case "mine":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutmineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Mining tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6225 * 16, 1007 * 16);
                        }
                    }
                    break;
                #endregion

                #region tuttrade
                case "trade":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuttraderegion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Trading tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6250 * 16, 1009 * 16);
                        }
                    }
                    break;
                #endregion

                #region tuthouse
                case "house":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tuthouseregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Housing tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6216 * 16, 1015 * 16);
                        }
                    }
                    break;
                #endregion

                #region dropretry
                case "dropretry":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.dropretryregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6216 * 16, 1015 * 16);
                        }
                    }
                    break;
                #endregion

                #region tutdrop
                case "drop":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.tutdropregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You need to be in the Tutorial zone, at the correct shaft of the Itemdrop tutorials.");
                            args.Player.SendErrorMessage("Stand on the sign when you execute the command.");
                            return;
                        }

                        else
                        {
                            args.Player.Teleport(6230 * 16, 1030 * 16);
                        }
                    }
                    break;
                    #endregion
            }
        }
        #endregion

        #region Trials
        private void Trial(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: At level 29, 59, 69 and 79 you have to complete a trial. Below you can find the available commands.", Color.Goldenrod);
                args.Player.SendMessage("Info: If you wish to skip the trials use /trial skip to get more information about it.", Color.Goldenrod);
                args.Player.SendMessage("Info: The commands to finish the trial can be found on the last sign of the trial.", Color.Goldenrod);
                args.Player.SendMessage("Info: You can monitor your progress with /trial progress.", Color.Goldenrod);
                args.Player.SendMessage("IMPORTANT: Skipping to level 70 is available for 35000 Terra Coins, but this is one way.", Color.Red);
                args.Player.SendMessage("You won't be able to come back and do the story elements when they come out with a 70+ character.", Color.Red);
                args.Player.SendMessage("Only skip to level 70 if you are absolutely sure about this.", Color.Red);
                return;
            }

            switch (args.Parameters[0])
            {
                #region lab1
                case "lab1":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.lab1region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.trial30magegroup || args.Player.Group.Name == Config.contents.trial30rangergroup || args.Player.Group.Name == Config.contents.trial30warriorgroup || args.Player.Group.Name == Config.contents.trial30summonergroup || args.Player.Group.Name == Config.contents.trial30terrariangroup)
                        {
                            if (args.Player.Group.Name == Config.contents.trial30magegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab1magegroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab1npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab1npc2);
                                var lab1player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc1, npc.name, 1, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc2, npc2.name, 8, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.lab1reward);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 10, 0);
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            }
                            if (args.Player.Group.Name == Config.contents.trial30rangergroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab1rangergroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab1npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab1npc2);
                                var lab1player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc1, npc.name, 1, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc2, npc2.name, 8, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.lab1reward);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 10, 0);
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            }
                            if (args.Player.Group.Name == Config.contents.trial30warriorgroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab1warriorgroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab1npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab1npc2);
                                var lab1player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc1, npc.name, 1, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc2, npc2.name, 8, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.lab1reward);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 10, 0);
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            }
                            if (args.Player.Group.Name == Config.contents.trial30summonergroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab1summonergroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab1npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab1npc2);
                                var lab1player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc1, npc.name, 1, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc2, npc2.name, 8, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.lab1reward);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 10, 0);
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            }
                            else if (args.Player.Group.Name == Config.contents.trial30terrariangroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab1terrariangroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab1npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab1npc2);
                                var lab1player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc1, npc.name, 1, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab1npc2, npc2.name, 8, lab1player.TSPlayer.TileX, lab1player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.lab1reward);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 10, 0);
                                args.Player.SendMessage("You just looted Worm Food and nothing else! It's a stinking hole, what did you expect?", Color.Goldenrod);
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You has to be level 29 to start the trial.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region lab2
                case "lab2":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.lab2region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Check the signs for hints.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.lab1magegroup || args.Player.Group.Name == Config.contents.lab1rangergroup || args.Player.Group.Name == Config.contents.lab1warriorgroup || args.Player.Group.Name == Config.contents.lab1summonergroup || args.Player.Group.Name == Config.contents.lab1terrariangroup)
                        {
                            if (args.Player.Group.Name == Config.contents.lab1magegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab2magegroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab2npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab2npc2);
                                var lab2player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc1, npc.name, 5, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc2, npc2.name, 3, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                args.Player.DamagePlayer(15);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);

                            }
                            if (args.Player.Group.Name == Config.contents.lab1rangergroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab2rangergroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab2npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab2npc2);
                                var lab2player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc1, npc.name, 5, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc2, npc2.name, 3, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                args.Player.DamagePlayer(15);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);

                            }
                            if (args.Player.Group.Name == Config.contents.lab1warriorgroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab2warriorgroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab2npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab2npc2);
                                var lab2player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc1, npc.name, 5, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc2, npc2.name, 3, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                args.Player.DamagePlayer(15);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);

                            }
                            if (args.Player.Group.Name == Config.contents.lab1summonergroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab2summonergroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab2npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab2npc2);
                                var lab2player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc1, npc.name, 5, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc2, npc2.name, 3, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                args.Player.DamagePlayer(15);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);

                            }
                            else if (args.Player.Group.Name == Config.contents.lab1terrariangroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.lab2terrariangroup);
                                var npc = TShock.Utils.GetNPCById(Config.contents.lab2npc1);
                                var npc2 = TShock.Utils.GetNPCById(Config.contents.lab2npc2);
                                var lab2player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc1, npc.name, 5, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                TSPlayer.Server.SpawnNPC(Config.contents.lab2npc2, npc2.name, 3, lab2player.TSPlayer.TileX, lab2player.TSPlayer.TileY);
                                args.Player.SendMessage("The air gets colder after you touch the stone. A loud laughter echoes from the stone and you reach for your face!", Color.Goldenrod);
                                args.Player.DamagePlayer(15);
                                TSPlayer.All.SendMessage(args.Player.Name + " slapped himself. Muhahahahaha", Color.Goldenrod);

                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to complete the frist part of the trial before this.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Trial 30
                case "trial30":
                    {
                        #region Trial 30 region check
                        Region region = TShock.Regions.GetRegionByName(Config.contents.trial30region);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check story signs to get hints.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check story signs to get hints.");
                            return;
                        }
                        #endregion

                        #region Trial 30 group check
                        if (args.Player.Group.Name == Config.contents.lab2magegroup || args.Player.Group.Name == Config.contents.lab2rangergroup || args.Player.Group.Name == Config.contents.lab2warriorgroup || args.Player.Group.Name == Config.contents.lab2summonergroup || args.Player.Group.Name == Config.contents.lab2terrariangroup)
                        {
                            #endregion

                            #region Trial 30 mage
                            if (args.Player.Group.Name == Config.contents.lab2magegroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Sorcerer", Color.SkyBlue);
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial30magefinish);
                                int type = 167;
                                int f = Projectile.NewProjectile(args.Player.TPlayer.position.X, args.Player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.trial30npc);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.trial30npc, npc.name, 5, trial30player.TSPlayer.TileX, trial30player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.trial30item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.trial30item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 4, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.trial30item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 9, 0);
                            }
                            #endregion

                            #region Trial 30 ranger
                            if (args.Player.Group.Name == Config.contents.lab2rangergroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Marksman", Color.SkyBlue);
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial30rangerfinish);
                                int type = 167;
                                int f = Projectile.NewProjectile(args.Player.TPlayer.position.X, args.Player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.trial30npc);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.trial30npc, npc.name, 5, trial30player.TSPlayer.TileX, trial30player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.trial30item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.trial30item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 4, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.trial30item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 9, 0);
                            }
                            #endregion

                            #region Trial 30 warrior
                            if (args.Player.Group.Name == Config.contents.lab2warriorgroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Knight", Color.SkyBlue);
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial30warriorfinish);
                                int type = 167;
                                int f = Projectile.NewProjectile(args.Player.TPlayer.position.X, args.Player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.trial30npc);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.trial30npc, npc.name, 5, trial30player.TSPlayer.TileX, trial30player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.trial30item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.trial30item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 4, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.trial30item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 9, 0);
                            }
                            #endregion

                            #region Trial 30 summoner
                            if (args.Player.Group.Name == Config.contents.lab2summonergroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Beckoner", Color.SkyBlue);
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial30summonerfinish);
                                int type = 167;
                                int f = Projectile.NewProjectile(args.Player.TPlayer.position.X, args.Player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.trial30npc);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.trial30npc, npc.name, 5, trial30player.TSPlayer.TileX, trial30player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.trial30item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.trial30item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 4, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.trial30item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 9, 0);
                            }
                            #endregion

                            #region Trial 30 terrarian
                            if (args.Player.Group.Name == Config.contents.lab2terrariangroup && args.Player.CurrentRegion == region)
                            {
                                args.Player.SendMessage("Congratulations! You have solved the riddles and completed the trial.", Color.Goldenrod);
                                args.Player.SendMessage("Rifling through the Nencromancer's corpse you find some loot!", Color.Goldenrod);
                                TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Terrarian", Color.SkyBlue);
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial30terrarianfinish);
                                int type = 167;
                                int f = Projectile.NewProjectile(args.Player.TPlayer.position.X, args.Player.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.trial30npc);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.trial30npc, npc.name, 5, trial30player.TSPlayer.TileX, trial30player.TSPlayer.TileY);
                                Item itemById = TShock.Utils.GetItemById(Config.contents.trial30item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.trial30item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 4, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.trial30item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 9, 0);
                            }
                            #endregion
                        }

                        else
                        {
                            args.Player.SendErrorMessage("You need to complete the second part of the trial before this.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Trialprogress
                case "progress":
                    {
                        if (args.Player.Group.Name == Config.contents.trial30magegroup || args.Player.Group.Name == Config.contents.trial30warriorgroup || args.Player.Group.Name == Config.contents.trial30rangergroup || args.Player.Group.Name == Config.contents.trial30summonergroup || args.Player.Group.Name == Config.contents.trial30terrariangroup)
                        {
                            args.Player.SendInfoMessage("You are level 29 and not yet completed any part of the level 30 trial.");
                            args.Player.SendInfoMessage("If you are stuck, go back to the boarding house at Landfall to get some info.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.lab1magegroup || args.Player.Group.Name == Config.contents.lab1warriorgroup || args.Player.Group.Name == Config.contents.lab1rangergroup || args.Player.Group.Name == Config.contents.lab1summonergroup || args.Player.Group.Name == Config.contents.lab1terrariangroup)
                        {
                            args.Player.SendInfoMessage("You have comnpleted the first part of the level 30 trial. You don't need to do it again, find the second part.");
                            args.Player.SendInfoMessage("If you need info about the location, go back to the first lab and read the notes in the cave.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.lab2magegroup || args.Player.Group.Name == Config.contents.lab2warriorgroup || args.Player.Group.Name == Config.contents.lab2rangergroup || args.Player.Group.Name == Config.contents.lab2summonergroup || args.Player.Group.Name == Config.contents.lab2terrariangroup)
                        {
                            args.Player.SendInfoMessage("You have comnpleted the first and second part of the level 30 trial. You don't need to do those again, find the third, last part.");
                            args.Player.SendInfoMessage("If you need info about the location, go back to the second lab and read the notes in the desert lab.");
                            return;
                        }
                        else
                        {
                            args.Player.SendInfoMessage("You are not level 29.");
                        }
                    }
                    break;
                #endregion

                #region Trial 60 shrine
                case "shrine":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.trial60shrineregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check Geralt's house for more info.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check Geralt's house for more info.");
                            return;
                        }
                        if (args.Player.Group.Name == Config.contents.trial60mageshrinegroup || args.Player.Group.Name == Config.contents.trial60warriorshrinegroup || args.Player.Group.Name == Config.contents.trial60rangershrinegroup || args.Player.Group.Name == Config.contents.trial60summonershrinegroup || args.Player.Group.Name == Config.contents.trial60terrarianshrinegroup)
                        {
                            if (args.Player.Group.Name == Config.contents.trial60mageshrinegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial60magegroup);
                                args.Player.SendErrorMessage("You touch the shrine with great hopes, but nothing happens at all. What a disappointment.");
                            }
                            if (args.Player.Group.Name == Config.contents.trial60warriorshrinegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial60warriorgroup);
                                args.Player.SendErrorMessage("You touch the shrine with great hopes, but nothing happens at all. What a disappointment.");
                            }
                            if (args.Player.Group.Name == Config.contents.trial60rangershrinegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial60rangergroup);
                                args.Player.SendErrorMessage("You touch the shrine with great hopes, but nothing happens at all. What a disappointment.");
                            }
                            if (args.Player.Group.Name == Config.contents.trial60summonershrinegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial60summonergroup);
                                args.Player.SendErrorMessage("You touch the shrine with great hopes, but nothing happens at all. What a disappointment.");
                            }
                            else if (args.Player.Group.Name == Config.contents.trial60terrarianshrinegroup)
                            {
                                var player = TShock.Users.GetUserByName(args.Player.User.Name);
                                TShock.Users.SetUserGroup(player, Config.contents.trial60terrariangroup);
                                args.Player.SendErrorMessage("You touch the shrine with great hopes, but nothing happens at all. What a disappointment.");
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You are not level 69.");
                                return;
                            }
                        }
                    }
                    break;
                #endregion

                #region Trial 60
                case "trial60":
                    {
                        #region Trial 60 group check
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            #endregion

                            #region Trial 60 mage
                            if (args.Player.Group.Name == Config.contents.trial60magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60mageregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60magegroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                        TShock.Users.SetUserGroup(trialuser, Config.contents.trial60magefinish);
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Archon", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 ranger
                            if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60rangerregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Ranger room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Ranger room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60rangergroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                        TShock.Users.SetUserGroup(trialuser, Config.contents.trial60rangerfinish);
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Deadshot", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 warrior
                            if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60warriorregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Warrior room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Warrior room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60warriorgroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                        TShock.Users.SetUserGroup(trialuser, Config.contents.trial60warriorfinish);
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Blademaster", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 summoner
                            if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60summonerregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60summonergroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                        TShock.Users.SetUserGroup(trialuser, Config.contents.trial60summonerfinish);
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Animist", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion

                            #region Trial 60 terrarian
                            if (args.Player.Group.Name == Config.contents.trial60terrariangroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trial60terrarianregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Requirement: Tomb of the Necrodancder, Mage room.");
                                    return;
                                }
                                if (args.Player.Group.Name == Config.contents.trial60terrariangroup && args.Player.CurrentRegion == region)
                                {
                                    var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                    var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                    var playeramount = selectedPlayer.Balance;
                                    var player = Playerlist[args.Player.Index];
                                    Money moneyamount = -Config.contents.trial60cost;
                                    Money moneyamount2 = Config.contents.trial60cost;
                                    if (playeramount < moneyamount2)
                                    {
                                        args.Player.SendErrorMessage("You need {0} to use this command. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                        return;
                                    }

                                    else
                                    {
                                        SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial.", moneyamount2, args.Player.Name), string.Format("Level 60 trial"));
                                        var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                        TShock.Users.SetUserGroup(trialuser, Config.contents.trial60terrarianfinish);
                                        TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Terrarian", Color.SkyBlue);
                                        args.Player.SendMessage("You have paid 12 000 Terra Coins for the level 60 trial", Color.Goldenrod);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59.");
                            return;
                        }
                    }
                    break;

                #endregion                

                #region Trial skip

                #region Skipinfo
                case "skip":
                    {
                        args.Player.SendMessage("You can skip the trials but it iwll cost you a hefty amount of Terra Coins.", Color.Goldenrod);
                        args.Player.SendMessage("The level 30 trial skip costs 50 000 Terra Coins.", Color.Goldenrod);
                        args.Player.SendMessage("The level 60 trial skip costs 200 000 Terra Coins.", Color.Goldenrod);
                        args.Player.SendMessage("To avoid accidental skips you need to be at Melody's Farmstead in the room marked with a T.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region Level 30 trial skip
                case "skip30":
                    {
                        if (args.Player.Group.Name == Config.contents.trial30magegroup || args.Player.Group.Name == Config.contents.trial30rangergroup || args.Player.Group.Name == Config.contents.trial30warriorgroup || args.Player.Group.Name == Config.contents.trial30summonergroup || args.Player.Group.Name == Config.contents.trial30terrariangroup)
                        {
                            #region Trial 30 mage skip
                            if (args.Player.Group.Name == Config.contents.trial30magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial30magefinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Sorcerer", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 ranger skip
                            if (args.Player.Group.Name == Config.contents.trial30rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial30rangerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Marksman", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 warrior skip
                            if (args.Player.Group.Name == Config.contents.trial30warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial30warriorfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Knight", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 summoner skip
                            if (args.Player.Group.Name == Config.contents.trial30summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial30summonerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Beckoner", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 30 terrarian skip
                            if (args.Player.Group.Name == Config.contents.trial30terrariangroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 50 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial30skipcost;
                                Money moneyamount2 = Config.contents.trial30skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 30 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 30 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 30 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial30terrarianfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.30.Terrarian", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 50 000 Terra Coins for the level 30 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion
                        }

                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 29.");
                            return;
                        }

                    }
                    break;
                #endregion                

                #region Level 60 trial skip
                case "skip60":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            #region Trial 60 mage skip
                            if (args.Player.Group.Name == Config.contents.trial60magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial60magefinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Archon", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 ranger skip
                            if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial60rangerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Deadshot", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 warrior skip
                            if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial60warriorfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Blademaster", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 summoner skip
                            if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial60summonerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Animist", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 terrarian skip
                            if (args.Player.Group.Name == Config.contents.trial60terrariangroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 200 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial60skipcost;
                                Money moneyamount2 = Config.contents.trial60skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 60 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 60 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 60 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial60terrarianfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.60.Terrarian", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 200 000 Terra Coins for the level 60 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 59.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Level 70 trial skip
                case "skip70":
                    {
                        if (args.Player.Group.Name == Config.contents.trial70magegroup || args.Player.Group.Name == Config.contents.trial70warriorgroup || args.Player.Group.Name == Config.contents.trial70rangergroup || args.Player.Group.Name == Config.contents.trial70summonergroup || args.Player.Group.Name == Config.contents.trial70terrariangroup)
                        {
                            #region Trial 70 mage skip
                            if (args.Player.Group.Name == Config.contents.trial70magegroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial70skipcost;
                                Money moneyamount2 = Config.contents.trial70skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 70 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 70 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 70 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial70magefinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.70.Magic.Incarnate", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 35 000 Terra Coins for the level 70 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 70 ranger skip
                            if (args.Player.Group.Name == Config.contents.trial70rangergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial70skipcost;
                                Money moneyamount2 = Config.contents.trial70skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 70 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 70 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 70 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial70rangerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.70.Ghost", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 35 000 Terra Coins for the level 70 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 70 warrior skip
                            if (args.Player.Group.Name == Config.contents.trial70warriorgroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial70skipcost;
                                Money moneyamount2 = Config.contents.trial70skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 70 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 70 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 70 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial70warriorfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.70.Mythical.Hero", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 35 000 Terra Coins for the level 70 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 70 summoner skip
                            if (args.Player.Group.Name == Config.contents.trial70summonergroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial70skipcost;
                                Money moneyamount2 = Config.contents.trial70skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 70 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 70 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 70 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial70summonerfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.70.Primal.Mover", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 35 000 Terra Coins for the level 70 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion

                            #region Trial 60 terrarian skip
                            if (args.Player.Group.Name == Config.contents.trial70terrariangroup)
                            {
                                Region region = TShock.Regions.GetRegionByName(Config.contents.trialskipregion);
                                if (args.Player.CurrentRegion != region)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }
                                if (args.Player.CurrentRegion == null)
                                {
                                    args.Player.SendErrorMessage("You are not in the right region. Look for a basement at Melody's Farmstead with the letter T.");
                                    args.Player.SendErrorMessage("This command will cost you 35 000 Terra Coins!");
                                    return;
                                }

                                var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                                var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                                var playeramount = selectedPlayer.Balance;
                                var player = Playerlist[args.Player.Index];
                                Money moneyamount = -Config.contents.trial70skipcost;
                                Money moneyamount2 = Config.contents.trial70skipcost;
                                if (playeramount < moneyamount2)
                                {
                                    args.Player.SendErrorMessage("You need {0} to skip the level 70 trial. You have {1}.", moneyamount2, selectedPlayer.Balance);
                                    return;
                                }

                                else
                                {
                                    SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the level 70 trial skip.", moneyamount2, args.Player.Name), string.Format("Level 70 trial skip"));
                                    var trialuser = TShock.Users.GetUserByName(args.Player.User.Name);
                                    TShock.Users.SetUserGroup(trialuser, Config.contents.trial70terrarianfinish);
                                    TSPlayer.All.SendMessage(args.Player.Name + " has become a Level.70.Terrarian", Color.SkyBlue);
                                    args.Player.SendMessage("You have paid 35 000 Terra Coins for the level 70 trial skip", Color.Goldenrod);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 69.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Level 80 trial skip
                case "skip80":
                    {

                    }
                    break;
                #endregion

                #endregion

                #region Question 1
                case "european":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region && args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                            {
                                args.Player.DamagePlayer(9001);
                                args.Player.SendMessage("You are not very good with swallows are you?", Color.Goldenrod);
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59");
                            return;
                        }
                    }
                    break;

                case "african":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region)
                            {
                                if (args.Player.Group.Name == Config.contents.trial60magegroup)
                                {
                                    args.Player.Teleport(870 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                                {
                                    args.Player.Teleport(825 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                                {
                                    args.Player.Teleport(754 * 16, 1956 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                else if (args.Player.Group.Name == Config.contents.trial60terrariangroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Question 2
                case "bucharest":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region && args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                            {
                                args.Player.DamagePlayer(9001);
                                args.Player.SendMessage("Couldn't you at least google it?", Color.Goldenrod);
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59");
                            return;
                        }
                    }
                    break;

                case "budapest":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region)
                            {
                                if (args.Player.Group.Name == Config.contents.trial60magegroup)
                                {
                                    args.Player.Teleport(870 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                                {
                                    args.Player.Teleport(825 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                                {
                                    args.Player.Teleport(754 * 16, 1956 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                else if (args.Player.Group.Name == Config.contents.trial60terrariangroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Question 3
                case "brian":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region)
                            {
                                if (args.Player.Group.Name == Config.contents.trial60magegroup)
                                {
                                    args.Player.Teleport(870 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60warriorgroup)
                                {
                                    args.Player.Teleport(825 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60rangergroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                if (args.Player.Group.Name == Config.contents.trial60summonergroup)
                                {
                                    args.Player.Teleport(754 * 16, 1956 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                                else if (args.Player.Group.Name == Config.contents.trial60terrariangroup)
                                {
                                    args.Player.Teleport(836 * 16, 1955 * 16);
                                    args.Player.SendMessage("Maybe they can just hold it under the dorsal guiding feathers.", Color.Goldenrod);
                                }
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59.");
                            return;
                        }
                    }
                    break;

                case "steven":
                    {
                        if (args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.trial60questionregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. If you are in the level 60 trial zone stand on the blinking teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == region && args.Player.Group.Name == Config.contents.trial60magegroup || args.Player.Group.Name == Config.contents.trial60rangergroup || args.Player.Group.Name == Config.contents.trial60warriorgroup || args.Player.Group.Name == Config.contents.trial60summonergroup || args.Player.Group.Name == Config.contents.trial60terrariangroup)
                            {
                                args.Player.DamagePlayer(9001);
                                args.Player.SendMessage("Well, you can't stop Ninja Brian. This 9001 damage is the least of your worries.", Color.Goldenrod);
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You are not level 59");
                            return;
                        }
                    }
                    break;
                    #endregion
            }
        }
        #endregion

        #region Story

        private void Story(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("If you want to start the story use /teleport story.", Color.Goldenrod);
                args.Player.SendMessage("Sometimes you need to read the signs twice. There are hints that will lead you to the next part.", Color.Goldenrod);
                args.Player.SendMessage("For a tutorial on how to do the first part check www.geldar.net Tutorials forum.", Color.Goldenrod);
                args.Player.SendMessage("/story info will help you sort out the story.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Info
                case "info":
                    {
                        args.Player.SendMessage("The story starts at /teleport story.", Color.Goldenrod);
                        args.Player.SendMessage("The first part cosists of the greek ruins, asian village and the hidden treasure.", Color.SkyBlue);
                        args.Player.SendMessage("The second part is the level 29 trial, it starts at Landfall at the Boarding House", Color.SkyBlue);
                        args.Player.SendMessage("You need to do the trial in order.", Color.SkyBlue);
                        args.Player.SendMessage("The third part is the level 60 trial. It starts at Geralt's house in the housing area.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region greekone
                case "greekone":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.greekoneregion);
                        if (player.greekonecd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.greekonecd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead.");
                            return;
                        }
                        else
                        {
                            Item itemById = TShock.Utils.GetItemById(Config.contents.greekoneitem);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, itemById.stack, 0);
                            args.Player.SendMessage("You just looted an Ice Mirror", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.greekonecd = Config.contents.greekonecd;
                            }
                        }
                    }
                    break;
                #endregion

                #region hidden
                case "hidden":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hiddenregion);
                        if (player.hiddencd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hiddencd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        else
                        {
                            Item itemById = TShock.Utils.GetItemById(Config.contents.hiddenitem);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, itemById.stack, 0);
                            args.Player.SendMessage("You just looted Flurry Boots!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.hiddencd = Config.contents.hiddencd;
                            }
                        }
                    }
                    break;
                    #endregion
            }
        }

        #endregion

        #region Quests
        private void Quests(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Each quest has it's own subcommand, which can be found at the quest's destination.", Color.Goldenrod);
                args.Player.SendMessage("To finish some of the quests you need certain items to be in your inventory.", Color.Goldenrod);
                args.Player.SendMessage("You can check the available quests on the notice boards next to spawn.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region giro
                case "giro":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.giroregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.girocd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.girocd));
                            return;
                        }

                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Professor Giro's house.");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Professor Giro's house.");
                            return;
                        }

                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.giroreward, BankAccountTransferOptions.AnnounceToReceiver, "Professor Giro Reward", "Giro reward");
                            args.Player.SendMessage("You found 150 Terra Coins laying around in a chest.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.girocd = Config.contents.girocd;
                            }
                        }
                    }
                    break;
                #endregion

                #region mill
                case "mill":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.millregion);
                        if (player.millcd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.millcd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Melody's Farmstead.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Melody's Farmstead.");
                            return;
                        }
                        else
                        {
                            Item itemById = TShock.Utils.GetItemById(Config.contents.millitem);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                            args.Player.SendMessage("You just looted a Sapphire Hook!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.millcd = Config.contents.millcd;
                            }
                        }
                    }
                    break;

                #endregion

                #region gloomy cave
                case "cave":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.caveregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.cavecd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.cavecd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: The Forest of the Dead");
                            return;
                        }
                        else
                        {
                            Item itemById = TShock.Utils.GetItemById(Config.contents.caveitem2);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                            Item itemById2 = TShock.Utils.GetItemById(Config.contents.caveitem1);
                            args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 12, 0);
                            args.Player.SendMessage("You just looted a Gold Broadsword and 12 Gold Bars!", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.cavecd = Config.contents.cavecd;
                            }
                        }

                    }
                    break;

                #endregion

                #region Lab1 lava
                case "qlab1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.qlab1region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.qlab1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.qlab1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Lava Lab.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Lava Lab.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.qlab1reward, BankAccountTransferOptions.AnnounceToReceiver, "Professor Giro Lava Reward", "Giro Lava reward");
                            args.Player.SendMessage("You found 150 Terra Coins laying around in a chest.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.qlab1cd = Config.contents.qlab1cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Lab2 desert
                case "qlab2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.qlab2region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.qlab2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.qlab1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Desert Lab.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Desert Lab.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.qlab2reward, BankAccountTransferOptions.AnnounceToReceiver, "Professor Giro Desert Reward", "Giro Desert reward");
                            args.Player.SendMessage("You found 150 Terra Coins laying around in a chest.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.qlab2cd = Config.contents.qlab2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Lab3 snow
                case "qlab3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.qlab3region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.qlab3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.qlab3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Snow Lab.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Giro's Snow Lab.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.qlab3reward, BankAccountTransferOptions.AnnounceToReceiver, "Professor Giro Snow Reward", "Giro Snow reward");
                            args.Player.SendMessage("You found 150 Terra Coins laying around in a chest.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.qlab3cd = Config.contents.qlab3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Jungle shrine 1
                case "shrine1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.shrine1region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.shrine1cd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.shrine1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.shrine1reward, BankAccountTransferOptions.AnnounceToReceiver, "Shrine 1 reward", "Shrine 1 reward");
                            args.Player.SendMessage("The ancestors bless you. You loot some Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.shrine1cd = Config.contents.shrine1cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Jungle shrine 2
                case "shrine2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.shrine2region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.shrine2cd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.shrine2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.shrine2reward, BankAccountTransferOptions.AnnounceToReceiver, "Shrine 2 reward", "Shrine 2 reward");
                            args.Player.SendMessage("The ancestors bless you. You loot some Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.shrine2cd = Config.contents.shrine2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Jungle shrine 3
                case "shrine3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.shrine3region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.shrine3cd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.shrine3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.shrine3reward, BankAccountTransferOptions.AnnounceToReceiver, "Shrine 3 reward", "Shrine 3 reward");
                            args.Player.SendMessage("The ancestors bless you. You loot some Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.shrine3cd = Config.contents.shrine3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Jungle shrine 4
                case "shrine4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.shrine4region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.shrine4cd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.shrine4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.shrine4reward, BankAccountTransferOptions.AnnounceToReceiver, "Shrine 4 reward", "Shrine 4 reward");
                            args.Player.SendMessage("The ancestors bless you. You loot some Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.shrine4cd = Config.contents.shrine4cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Jungle shrine 5
                case "shrine5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.shrine5region);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.shrine5cd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.shrine5cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Correct shrine in normal jungle.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.shrine5reward, BankAccountTransferOptions.AnnounceToReceiver, "Shrine 5 reward", "Shrine 5 reward");
                            args.Player.SendMessage("The ancestors bless you. You loot some Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.shrine5cd = Config.contents.shrine5cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Viking ship
                case "viking":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.vikingregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.vikingcd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.vikingcd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Sunken viking ship.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: Sunken viking ship.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.vikingreward, BankAccountTransferOptions.AnnounceToReceiver, "Viking ship reward", "Viking ship reward");
                            Item itemById = TShock.Utils.GetItemById(Config.contents.vikingitem1);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                            args.Player.SendMessage("You opened a wooden chest and found a Viking Helmet and 100 Terra Coins.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.vikingcd = Config.contents.vikingcd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Volcano
                case "volcano":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.vulcanregion);
                        if (!args.Player.Group.HasPermission("house.use"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 20 to complete this quest.");
                            return;
                        }
                        if (player.vulcancd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.vulcancd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: The Volcano.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not int he right region. Requirement: The Volcano.");
                            return;
                        }
                        else
                        {
                            IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                            IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.vulcanreward, BankAccountTransferOptions.AnnounceToReceiver, "Volcano reward", "Volcano reward");
                            Item itemById = TShock.Utils.GetItemById(Config.contents.vulcanitem1);
                            args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 15, 0);
                            args.Player.SendMessage("You have found a lava proof chest and looted 15 Obsidian and 150 Terra Coins from it.", Color.Goldenrod);
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.vulcancd = Config.contents.vulcancd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Dungeon
                case "dungeon":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.dungeonregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.dungeoncd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.dungeoncd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.dungeonitem1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 50, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.dungeonitem2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 2, 0);
                                IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                                IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.dungeonreward, BankAccountTransferOptions.AnnounceToReceiver, "Giro last quest reward.", "Giro last quest reward.");
                                args.Player.SendMessage("It's really sad but why would you leave these here?", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.dungeoncd = Config.contents.dungeoncd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Overgrown
                case "overgrown":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.overgrownregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.overgrowncd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.overgrowncd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.overgrownitem);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                                IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.overgrownreward, BankAccountTransferOptions.AnnounceToReceiver, "Overgrown sidequest reward.", "Overgrown sidequest reward.");
                                args.Player.SendMessage("These Lihzahrds... Experimenting with bees? Not the beees!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.overgrowncd = Config.contents.overgrowncd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corrupted
                case "corrupted":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corruptedregion);
                        if (!args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to complete this quest.");
                            return;
                        }
                        if (player.corruptedcd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corruptedcd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.corrupteditem);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                                IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.corruptedreward, BankAccountTransferOptions.AnnounceToReceiver, "Corrupted sidequest reward.", "Corrupted sidequest reward.");
                                args.Player.SendMessage("Don't feed the worm after midnight.", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corruptedcd = Config.contents.corruptedcd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Frozen                
                case "frozen":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.frozenregion);
                        if (!args.Player.Group.HasPermission("tshock.world.modify"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 10 to complete this quest.");
                            return;
                        }
                        if (player.frozencd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.frozencd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.frozenitem);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                                IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.frozenreward, BankAccountTransferOptions.AnnounceToReceiver, "Frozen sidequest reward.", "Frozen sidequest reward.");
                                args.Player.SendMessage("Do you wanna build a snow... NOPE", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.frozencd = Config.contents.frozencd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Hive   
                /*             
            case "hive":
                {
                    var player = Playerlist[args.Player.Index];
                    Region region = TShock.Regions.GetRegionByName(Config.contents.hiveregion);
                    if (!args.Player.Group.HasPermission("tshock.world.modify"))
                    {
                        args.Player.SendErrorMessage("You need to be at least level 10 to complete this quest");
                        return;
                    }
                    if (player.hivecd > 0)
                    {
                        args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hivecd));
                        return;
                    }
                    if (args.Player.CurrentRegion != region)
                    {
                        args.Player.SendErrorMessage("You are not in the right region. Check the signs at spawn for hints.");
                        return;
                    }
                    if (args.Player.CurrentRegion == null)
                    {
                        args.Player.SendErrorMessage("You are not in the right region. Check the signs at spawn for hints.");
                        return;
                    }
                    else
                    {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hiveitem);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                var npc = TShock.Utils.GetNPCById(Config.contents.hivenpcid);
                                var trial30player = Playerlist[args.Player.Index];
                                TSPlayer.Server.SpawnNPC(Config.contents.hivenpcid, npc.name, Config.contents.hivenpcamount, player.TSPlayer.TileX, player.TSPlayer.TileY);
                                args.Player.SendMessage("", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hivecd = Config.contents.hivecd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                    }
                }                    
            break;
            */
                #endregion

                #region Highlander                
                case "highlander":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.highlanderregion);
                        if (!args.Player.Group.HasPermission("tshock.world.modify"))
                        {
                            args.Player.SendErrorMessage("You need to be at least level 10 to complete this quest.");
                            return;
                        }
                        if (player.highlandercd > 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.highlandercd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Check the hints at spawn.");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.highlanderitem);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                IBankAccount Server = SEconomyPlugin.Instance.GetBankAccount(TSServerPlayer.Server.User.ID);
                                IBankAccount Player = SEconomyPlugin.Instance.GetBankAccount(player.Index);
                                SEconomyPlugin.Instance.WorldAccount.TransferToAsync(Player, Config.contents.highlanderreward, BankAccountTransferOptions.AnnounceToReceiver, "Highlander sidequest reward.", "Highlander sidequest reward.");
                                args.Player.SendMessage("Heeeeere we areeee, born to be kings!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.highlandercd = Config.contents.highlandercd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                    #endregion
            }

        }

        #endregion

        #region Minigames
        private void Minigame(CommandArgs args)
        {
            args.Player.SendMessage("Not yet", Color.Goldenrod);
            return;
        }
        #endregion

        #region Teleport
        private void Teleport(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Use the commands below.", Color.Goldenrod);
                args.Player.SendMessage("Info: /teleport adventure - Teleports you to the adventure tower.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport tutorial - Teleports you to the Tutorial zone.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport story - Teleports you to the very first part of the story.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport oasis - Teleports you to the Poised Oasis.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport minigame - Teleports you to the minigame area. -In development-", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport vip1 - Teleports you to the above ground VIP housing.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport vip2 - Teleports you to the underground VIP housing.", Color.SkyBlue);
                args.Player.SendMessage("Info: /teleport lite - Teleports you to the Elite lite housing.", Color.SkyBlue);
                args.Player.SendMessage("Info: Oasis teleport requirements: 250 Terra coins, Level 30, Warehouse teleport pad.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Adventure teleport
                case "adventure":
                    {
                        if (args.Player.Group.HasPermission("geldar.level5"))
                        {
                            args.Player.Teleport(445 * 16, 875 * 16);
                            args.Player.SendMessage("You have been teleported to the Adventure Tower.", Color.Goldenrod);
                            args.Player.SendMessage("Remember to have at least 4 free inventory slots.", Color.Goldenrod);
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to start an Adventure");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Tutorial teleport
                case "tutorial":
                    {
                        args.Player.Teleport(6223 * 16, 962 * 16);
                        args.Player.SendMessage("Yoou have been teleported to the Tutorial zone.", Color.Goldenrod);
                    }
                    break;

                #endregion

                #region Oasis teleport
                case "oasis":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.oasiscost;
                        Money moneyamount2 = Config.contents.oasiscost;
                        if (!args.Player.Group.HasPermission("geldar.level30"))
                        {
                            args.Player.SendErrorMessage("You need to be level 30 to use this command.");
                            return;
                        }
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to teleport to the oasis. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }

                        Region region = TShock.Regions.GetRegionByName(Config.contents.oasisregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Warehouse Teleportpad");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Warehouse Teleportpad");
                            return;
                        }

                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for the oasis teleport.", moneyamount2, args.Player.Name), string.Format("Oasis Teleport"));
                            args.Player.Teleport(1345 * 16, 456 * 16);
                            args.Player.SendMessage("You've paid 250 Terra Coins to be teleported to the Poisoned Oasis.", Color.Goldenrod);
                        }
                    }
                    break;

                #endregion

                #region Story teleport
                case "story":
                    {
                        if (args.Player.Group.HasPermission("geldar.level5"))
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.storyregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Spawn/Landfall");
                                return;
                            }

                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Spawn/Landfall");
                                return;
                            }

                            else
                            {
                                args.Player.Teleport(6096 * 16, 659 * 16);
                                args.Player.SendMessage("You have been teleport to the start of the story.", Color.Goldenrod);
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be at least level 5 to start the story.");
                            return;
                        }
                    }
                    break;

                #endregion

                #region Minigame
                case "minigame":
                    {
                        if (args.Player.Group.HasPermission("geldar.mod"))
                        {
                            args.Player.SendErrorMessage("No area define for teleport.");
                        }
                        else
                        {
                            args.Player.SendErrorMessage("Minigames are currently in development. Be patient.");
                        }
                    }
                    break;
                #endregion

                #region VIP1 teleport
                case "vip1":
                    {
                        args.Player.Teleport(5690 * 16, 543 * 16);
                        args.Player.SendMessage("You have been teleported to the aboveground VIP housing.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region VIP2 teleport
                case "vip2":
                    {
                        args.Player.Teleport(5769 * 16, 834 * 16);
                        args.Player.SendMessage("You have been teleported to the underground VIP housing.", Color.Goldenrod);
                    }
                    break;

                #endregion

                #region Elite lite
                case "lite":
                    {
                        args.Player.Teleport(6174 * 16, 1328 * 16);
                        args.Player.SendMessage("You have been teleported to the Elite lite housing.", Color.Goldenrod);
                    }
                    break;
                    #endregion
            }
        }

        #endregion

        #region Adventure command
        private void Adv(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: Each adventure subcommand can be used at the sign, at the correct place.", Color.Goldenrod);
                args.Player.SendMessage("Example: You are in the pyramid at the first sign. /adventure pyramid1", Color.Goldenrod);
                args.Player.SendMessage("Info: Each command has a unique cooldown of one day(24 hours).", Color.Goldenrod);
                args.Player.SendMessage("Info: Be sure to have at least 4 free inventory slots!", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Pyramid teleport
                case "pyramid":
                    {
                        if (args.Player.Group.HasPermission("geldar.mod"))
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.pyramidtpregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Pyramid teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Pyramid teleporter.");
                                return;
                            }
                            if (args.Player.Group.HasPermission("geldar.level5") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                            {
                                args.Player.Teleport(238 * 16, 1355 * 16);
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You need to be level 5 for this adventure.");
                                return;
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("This adventure is disabled right now.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Ice teleport
                case "ice":
                    {
                        if (args.Player.Group.HasPermission("geldar.mod"))
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.icetpregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Ice adventure teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Ice adventure teleporter.");
                                return;
                            }
                            if (args.Player.Group.HasPermission("geldar.level5") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                            {
                                args.Player.Teleport(112 * 16, 873 * 16);
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You need to be level 5 for this adventure.");
                                return;
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("This adventure is disabled right now.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Corr teleport
                case "corr":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corrtpregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Corruption adventure teleporter.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Corruption adventure teleporter.");
                            return;
                        }
                        if (args.Player.Group.HasPermission("geldar.level30") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                        {
                            args.Player.Teleport(138 * 16, 915 * 16);
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 30 for this adventure.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Crim teleport
                case "crim":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crimtpregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Crimson adventure teleporter.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Crimson adventure teleporter.");
                            return;
                        }
                        if (args.Player.Group.HasPermission("geldar.level30") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                        {
                            args.Player.Teleport(212 * 16, 1151 * 16);
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 30 for this adventure.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Jadv teleport
                case "jadv":
                    {
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadvtpregion);
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Jungle adventure teleporter.");
                            return;
                        }
                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Jungle adventure teleporter.");
                            return;
                        }
                        if (args.Player.Group.HasPermission("house.use") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                        {
                            args.Player.Teleport(355 * 16, 1164 * 16);
                        }
                        else
                        {
                            args.Player.SendErrorMessage("You need to be level 20 for this adventure.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Space teleport
                case "space":
                    {
                        if (args.Player.Group.HasPermission("geldar.mod"))
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.spacetpregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Space adventure teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Space adventure teleporter.");
                                return;
                            }
                            if (args.Player.Group.HasPermission("tshock.world.modify") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                            {
                                args.Player.Teleport(520 * 16, 138 * 16);
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You need to be level 10 for this adventure.");
                                return;
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("This adventure is disabled right now.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region Hallow teleport
                case "hallow":
                    {
                        if (args.Player.Group.HasPermission("geldar.mod"))
                        {
                            Region region = TShock.Regions.GetRegionByName(Config.contents.hallowtpregion);
                            if (args.Player.CurrentRegion != region)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Hallow adventure teleporter.");
                                return;
                            }
                            if (args.Player.CurrentRegion == null)
                            {
                                args.Player.SendErrorMessage("You are not in the right region. Requirement: Adventure tower, Hallow adventure teleporter.");
                                return;
                            }
                            if (args.Player.Group.HasPermission("tshock.world.modify") || args.Player.Group.HasPermission("geldar.vip") && args.Player.CurrentRegion == region)
                            {
                                args.Player.Teleport(786 * 16, 252 * 16);
                            }
                            else
                            {
                                args.Player.SendErrorMessage("You need to be level 10 for this adventure.");
                                return;
                            }
                        }
                        else
                        {
                            args.Player.SendErrorMessage("This adventure is disabled right now.");
                            return;
                        }
                    }
                    break;
                #endregion

                #region pyramid1
                case "pyramid1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid1region);
                        if (player.pyramid1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid1cd = Config.contents.pyramid1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems to be full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid2
                case "pyramid2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid2region);
                        if (player.pyramid2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid2item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 40, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 40 Meteories and an Omega Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid2cd = Config.contents.pyramid2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid3
                case "pyramid3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid3region);
                        if (player.pyramid3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid3item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                Item itemById4 = TShock.Utils.GetItemById(Config.contents.pyramid3item4);
                                args.Player.GiveItem(itemById4.type, itemById4.name, itemById4.width, itemById4.height, 5, 0);
                                args.Player.SendMessage("You just looted a Golden Key, an Aglet an Ankh Banner and 5 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid3cd = Config.contents.pyramid3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid4
                case "pyramid4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid4region);
                        if (player.pyramid4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                Item itemById4 = TShock.Utils.GetItemById(Config.contents.pyramid4item4);
                                args.Player.GiveItem(itemById4.type, itemById4.name, itemById4.width, itemById4.height, 1, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Sandstorm in a Bottle, and Omega Banner and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid4cd = Config.contents.pyramid4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid5
                case "pyramid5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid5region);
                        if (player.pyramid5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid5cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid5item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid5item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid5item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                Item itemById4 = TShock.Utils.GetItemById(Config.contents.pyramid5item4);
                                args.Player.GiveItem(itemById4.type, itemById4.name, itemById4.width, itemById4.height, 10, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Pharaoh Set and 10 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid5cd = Config.contents.pyramid5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid6
                case "pyramid6":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid6region);
                        if (player.pyramid6cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid6cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid6item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid6item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid6item2);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Band of Regeneration and an Omega Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid6cd = Config.contents.pyramid6cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid7
                case "pyramid7":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid7region);
                        if (player.pyramid7cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid7cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid7item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid7item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid7item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid7cd = Config.contents.pyramid7cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region pyramid8
                case "pyramid8":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.pyramid8region);
                        if (player.pyramid8cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.pyramid8cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Pyramid adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.pyramid8item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.pyramid8item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.pyramid8item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Shiny Red Balloon and a Snake Banner!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.pyramid8cd = Config.contents.pyramid8cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice1
                case "ice1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice1region);
                        if (player.ice1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Snowball Cannon and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice1cd = Config.contents.ice1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice2
                case "ice2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice2region);
                        if (player.ice2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice2item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, Ice Skates and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice2cd = Config.contents.ice2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice3
                case "ice3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice3region);
                        if (player.ice3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice3item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, a Blizzard in a Bottle and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice3cd = Config.contents.ice3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice4
                case "ice4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice4region);
                        if (player.ice4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, an Ice Blade and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice4cd = Config.contents.ice4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice5
                case "ice5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice5region);
                        if (player.ice5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice5cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice5item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice5item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice5item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, an Extractinator and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice5cd = Config.contents.ice5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region ice6
                case "ice6":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.ice6region);
                        if (player.ice6cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.ice6cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Ice adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.ice6item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.ice6item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.ice6item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 3, 0);
                                args.Player.SendMessage("You just looted a Golden Key, an Ice Boomerang and 3 Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.ice6cd = Config.contents.ice6cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr1
                case "corr1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr1region);
                        if (player.corr1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.corr1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.corr1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 15 Cursed Flames !", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr1cd = Config.contents.corr1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr2
                case "corr2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr2region);
                        if (player.corr2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.corr2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.corr2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.corr2item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 10, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, a Mechanical Worm and 10 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr2cd = Config.contents.corr2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr3
                case "corr3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr3region);
                        if (player.corr3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.corr3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.corr3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 10, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 10 Hallowed Bars!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr3cd = Config.contents.corr3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Corr4
                case "corr4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.corr4region);
                        if (player.corr4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.corr4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Corruption adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.corr4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.corr4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.corr4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and a Reaper Costume!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.corr4cd = Config.contents.corr4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim1
                case "crim1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim1region);
                        if (player.crim1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.crim1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.crim1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.crim1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, a Mechanical Worm and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim1cd = Config.contents.crim1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim2
                case "crim2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim2region);
                        if (player.crim2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.crim2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.crim2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 10, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 10 Hallowed Bars!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim2cd = Config.contents.crim2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim3
                case "crim3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim3region);
                        if (player.crim3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.crim3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.crim3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and 15 Ichor!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim3cd = Config.contents.crim3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Crim4
                case "crim4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.crim4region);
                        if (player.crim4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.crim4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimson adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Crimsom adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.crim4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 3, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.crim4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.crim4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                Item itemById4 = TShock.Utils.GetItemById(Config.contents.crim4item4);
                                args.Player.GiveItem(itemById4.type, itemById4.name, itemById4.width, itemById4.height, 1, 0);
                                args.Player.SendMessage("You just looted 3 Golden Keys, and a Space Creature Costume!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.crim4cd = Config.contents.crim4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv1
                case "jadv1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv1region);
                        if (player.jadv1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.jadv1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.jadv1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.jadv1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 1, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys a Rich Mahogany Leaf Wand and Living Mahogany Wand!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv1cd = Config.contents.jadv1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv2
                case "jadv2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv2region);
                        if (player.jadv2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.jadv2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.jadv2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys and Seaweed!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv2cd = Config.contents.jadv2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv3
                case "jadv3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv3region);
                        if (player.jadv3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.jadv3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.jadv3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Guide to Plant Fiber Cordage!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv3cd = Config.contents.jadv3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv4
                case "jadv4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv4region);
                        if (player.jadv4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.jadv4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.jadv4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Boomstick!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv4cd = Config.contents.jadv4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Jadv5
                case "jadv5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.jadv5region);
                        if (player.jadv5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.jadv5cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Jungle adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.jadv5item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.jadv5item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys and a Honey Dispenser!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.jadv5cd = Config.contents.jadv5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion

                #region Space1
                case "space1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space1region);
                        if (player.space1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.space1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.space1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 10, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.space1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 15, 0);
                                args.Player.SendMessage("You just looted a Golden Key, 10 Meteorite Bars, and 5 Emeralds!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space1cd = Config.contents.space1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space2
                case "space2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space2region);
                        if (player.space2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.space2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.space2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 10, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.space2item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted a Golden Key, 10 Fallen Stars, and 5 Amethysts!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space2cd = Config.contents.space2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space3
                case "space3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space3region);
                        if (player.space3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.space3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.space3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 10, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.space3item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted a Golden Key, 10 Fallen Stars, and 5 Sapphires!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space3cd = Config.contents.space3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Space4
                case "space4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.space4region);
                        if (player.space4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.space4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.space4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 1, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.space4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.space4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted a Golden Key, 15 Meteorite Bars, and 5 Rubies!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.space4cd = Config.contents.space4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow1
                case "hallow1":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow1region);
                        if (player.hallow1cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow1cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hallow1item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.hallow1item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.hallow1item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow1cd = Config.contents.hallow1cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow2
                case "hallow2":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow2region);
                        if (player.hallow2cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow2cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hallow2item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.hallow2item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.hallow2item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 15, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 15 Happy Grenades!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow2cd = Config.contents.hallow2cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow3
                case "hallow3":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow3region);
                        if (player.hallow3cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow3cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hallow3item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.hallow3item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 1, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.hallow3item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 5 Greater Healing Potions, and a Goodie Bag!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow3cd = Config.contents.hallow3cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow4
                case "hallow4":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow4region);
                        if (player.hallow4cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow4cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hallow4item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.hallow4item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.hallow4item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow4cd = Config.contents.hallow4cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                #endregion                

                #region Hallow5
                case "hallow5":
                    {
                        var player = Playerlist[args.Player.Index];
                        Region region = TShock.Regions.GetRegionByName(Config.contents.hallow5region);
                        if (player.hallow5cd != 0)
                        {
                            args.Player.SendErrorMessage("This command is on cooldown for {0} seconds.", (player.hallow5cd));
                            return;
                        }
                        if (args.Player.CurrentRegion != region)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Space adventure");
                            return;
                        }

                        if (args.Player.CurrentRegion == null)
                        {
                            args.Player.SendErrorMessage("You are not in the right region. Requirement: Hallow adventure");
                            return;
                        }
                        else
                        {
                            if (args.Player.InventorySlotAvailable)
                            {
                                Item itemById = TShock.Utils.GetItemById(Config.contents.hallow5item1);
                                args.Player.GiveItem(itemById.type, itemById.name, itemById.width, itemById.height, 2, 0);
                                Item itemById2 = TShock.Utils.GetItemById(Config.contents.hallow5item2);
                                args.Player.GiveItem(itemById2.type, itemById2.name, itemById2.width, itemById2.height, 15, 0);
                                Item itemById3 = TShock.Utils.GetItemById(Config.contents.hallow5item3);
                                args.Player.GiveItem(itemById3.type, itemById3.name, itemById3.width, itemById3.height, 5, 0);
                                args.Player.SendMessage("You just looted 2 Golden Keys, 15 Pumpkins, and 5 Greater Healing Potions!", Color.Goldenrod);
                                if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                                {
                                    player.hallow5cd = Config.contents.hallow5cd;
                                }
                            }
                            else
                            {
                                args.Player.SendErrorMessage("Your inventory seems full. Have at least 4 free slots.");
                            }
                        }
                    }
                    break;
                    #endregion
            }
        }
        #endregion   

        #region VIP
        private void VIP(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Use the commands below.", Color.Goldenrod);
                args.Player.SendMessage("/teleport vip1 - Teleports you to the aboveground vip housing.", Color.SkyBlue);
                args.Player.SendMessage("/teleport vip2 - Teleports you to the underground vip housing.", Color.SkyBlue);
                args.Player.SendMessage("/vip info - General Info about VIP ranks.", Color.SkyBlue);
                args.Player.SendMessage("/vip <elite/lite/champion/king/supreme/ultimate>", Color.SkyBlue);
                args.Player.SendMessage("You can contact us at admin@geldar.net", Color.SkyBlue);
                args.Player.SendMessage("Press enter and use the up arrow to scroll the chat.", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Info
                case "info":
                    {
                        args.Player.SendMessage("Here are some basic things to know about the VIP status.", Color.Goldenrod);
                        args.Player.SendMessage("As a VIP you still need to folow the rules. You can check some of them at /geldar.", Color.SkyBlue);
                        args.Player.SendMessage("If you make a non-VIP character you still need to use your VIP house with that character.", Color.SkyBlue);
                        args.Player.SendMessage("You can add your non-VIP to your VIP house with /house allow \"player name>\" \"house name\".", Color.SkyBlue);
                        args.Player.SendMessage("You can contact us at admin@geldar.net or on the forums at www.geldar.net.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region Lite
                case "lite":
                    {
                        args.Player.SendMessage("You are allowed to have a 12x15 house with 7 storage containers.", Color.Goldenrod);
                        args.Player.SendMessage("You can join anytime, even if the server if full.", Color.SkyBlue);
                        args.Player.SendMessage("Terrarian rank name.", Color.SkyBlue);
                        args.Player.SendMessage("Full leveling experience with trials and leveling rewards.", Color.SkyBlue);
                        args.Player.SendMessage("You can start invasion with items from level 20", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region Elite
                case "elite":
                    {
                        args.Player.SendMessage("You are allowed to have a 20x20 house with no chest amount restriction.", Color.Goldenrod);
                        args.Player.SendMessage("You can join anytime, even if the server if full.", Color.SkyBlue);
                        args.Player.SendMessage("Elite prefix and Royal-blue chat color.", Color.SkyBlue);
                        args.Player.SendMessage("No leveling system, no item restirction, you can start invasions with items.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region Champion
                case "champion":
                    {
                        args.Player.SendMessage("All the Elite rank benefits.", Color.Goldenrod);
                        args.Player.SendMessage("You are allowed to have a 25x25 house with no chest amount restriction.", Color.SkyBlue);
                        args.Player.SendMessage("You can summon the Collector's Edition Bunny with /bunny.", Color.SkyBlue);
                        args.Player.SendMessage("Champion prefix and Orange chat color.", Color.SkyBlue);
                        args.Player.SendMessage("Teleport back where you died with /b.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region King
                case "king":
                    {
                        args.Player.SendMessage("All the Elite and Champion rank benefits.", Color.Goldenrod);
                        args.Player.SendMessage("You are allowed to have a 30x30 house with no chest amount restriction.", Color.SkyBlue);
                        args.Player.SendMessage("King prefix and chat color of your liking.", Color.SkyBlue);
                        args.Player.SendMessage("You can use /home and /tp with all subcommands.", Color.SkyBlue);
                        args.Player.SendMessage("For available buff commands for your rank use /buffme.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region Supreme
                case "supreme":
                    {
                        args.Player.SendMessage("All the Elite, Champion and King rank benefits.", Color.Goldenrod);
                        args.Player.SendMessage("You are allowed to have a 30x30 house with no chest amount restriction.", Color.SkyBlue);
                        args.Player.SendMessage("One pet of your choice.", Color.SkyBlue);
                        args.Player.SendMessage("For available buff commands for your rank use /buffme.", Color.SkyBlue);
                    }
                    break;
                #endregion

                #region Ultimate
                case "ultimate":
                    {
                        args.Player.SendMessage("All the Elite, Champion, King and Supreme rank benefits.", Color.Goldenrod);
                        args.Player.SendMessage("You are allowed to have a 30x30 house with no chest amount restriction.", Color.SkyBlue);
                        args.Player.SendMessage("One mount of your choice.", Color.SkyBlue);
                        args.Player.SendMessage("For available buff commands for your rank use /buffme.", Color.SkyBlue);
                    }
                    break;
                    #endregion
            }
        }
        #endregion

        #region Buffme
        private void Buffme(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Use the commands below to buff youself. Minimum rank for the commands is King.", Color.Goldenrod);
                args.Player.SendMessage("Regular player buffs 150TC: Night/Swiftness/Waterwalking", Color.Goldenrod);
                args.Player.SendMessage("Regular player buffs 200TC: Builder/Calming/Dangersense/Flipper/Gills/Warmth/Hunter/Heartreach/Gravitation", Color.Goldenrod);
                args.Player.SendMessage("Regular player buffs 350TC: Shine/Archery/Ammores/Featherfall/Battle/Mining/Wrath", Color.Goldenrod);
                args.Player.SendMessage("350TC: Titan/Thorns/Summoning/Regeneration/Rage/Obsidian/Manareg/Magicpower/Lifeforce", Color.Goldenrod);
                args.Player.SendMessage("350TC: Ironskin/Inferno/Endurance", Color.Goldenrod);
                args.Player.SendMessage("Regular player buffs 500TC: Spelunker", Color.Goldenrod);
                args.Player.SendMessage("Info: /buffme sixthsense - Required rank: King or above.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme defense - Require rank: Supreme or above.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme misc - Required rank: Supreme or above.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme melee - Required rank : Ultimate.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme ranged - Required rank : Ultimate.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme magic - Required rank : Ultimate.", Color.SkyBlue);
                args.Player.SendMessage("Info: /buffme summoner - Required rank : Ultimate.", Color.SkyBlue);
                args.Player.SendMessage("Press enter then use the up arrow to scroll the chat", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region VIP buffs

                #region Sixthsense
                case "sixthsense":
                    {
                        if (!args.Player.Group.HasPermission("geldar.king"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(12, 14400);
                            args.Player.SetBuff(11, 18000);
                            args.Player.SetBuff(17, 18000);
                            args.Player.SetBuff(111, 36000);
                            args.Player.SendMessage("You have been buffed with Night Owl, Hunter, Dangersense and Shine Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Defense
                case "defense":
                    {
                        if (!args.Player.Group.HasPermission("geldar.supreme"))
                        {
                            args.Player.
                                SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(5, 18000);
                            args.Player.SetBuff(1, 14400);
                            args.Player.SetBuff(113, 18000);
                            args.Player.SetBuff(124, 54000);
                            args.Player.SetBuff(3, 14400);
                            args.Player.SetBuff(114, 14400);
                            args.Player.SetBuff(2, 18000);
                            args.Player.SetBuff(116, 14400);
                            args.Player.SendMessage("You have been buffed with Obisidian Skin, Warmth, Inferno, Swiftness, Endurance, Regeneration, Lifeforce and Ironskin Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Misc
                case "misc":
                    {
                        if (!args.Player.Group.HasPermission("geldar.supreme"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(122, 14400);
                            args.Player.SetBuff(121, 28800);
                            args.Player.SetBuff(123, 10800);
                            args.Player.SetBuff(109, 28800);
                            args.Player.SetBuff(104, 28800);
                            args.Player.SetBuff(9, 18000);
                            args.Player.SetBuff(4, 7200);
                            args.Player.SetBuff(15, 18000);
                            args.Player.SetBuff(106, 18000);
                            args.Player.SendMessage("You have been buffed with Water Walking, Fishing, Crate, Sonar, Gills, Mining, Spelunker, Flipper and Calming Potion", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Melee
                case "melee":
                    {
                        if (!args.Player.Group.HasPermission("geldar.ultimate"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(115, 14400);
                            args.Player.SetBuff(108, 14400);
                            args.Player.SetBuff(14, 7200);
                            args.Player.SetBuff(117, 14400);
                            args.Player.SendMessage("You have been buffed with Rage, Titan, Thorns and Wrath Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Ranged
                case "ranged":
                    {
                        if (!args.Player.Group.HasPermission("geldar.ultimate"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(16, 14400);
                            args.Player.SetBuff(112, 25200);
                            args.Player.SetBuff(115, 14400);
                            args.Player.SetBuff(117, 14400);
                            args.Player.SendMessage("You  have been buffed with Archery, Ammo Reservation, Rage and Wrath Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Magic
                case "magic":
                    {
                        if (!args.Player.Group.HasPermission("geldar.ultimate"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(7, 7200);
                            args.Player.SetBuff(6, 25200);
                            args.Player.SetBuff(115, 14400);
                            args.Player.SetBuff(117, 14400);
                            args.Player.SendMessage("You  have been buffed with Magic Power, Mana Regeneration, Rage and Wrath Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #region Summoner
                case "summoner":
                    {
                        if (!args.Player.Group.HasPermission("geldar.ultimate"))
                        {
                            args.Player.SendErrorMessage("You don't have permission to use this buff command.");
                            return;
                        }
                        else
                        {
                            args.Player.SetBuff(6, 25200);
                            args.Player.SetBuff(110, 21600);
                            args.Player.SetBuff(115, 14400);
                            args.Player.SetBuff(117, 14400);
                            args.Player.SetBuff(14, 7200);
                            args.Player.SendMessage("You  have been buffed with Mana Regeneration, Summoning, Rage, Wrath and Thorn Potion.", Color.Goldenrod);
                        }
                    }
                    break;
                #endregion

                #endregion

                #region Regular player buffs 150TC

                #region Night
                case "night":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff1cost;
                        Money moneyamount2 = Config.contents.buff1cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff1cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff1cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 1 buff.", moneyamount2, args.Player.Name), string.Format("Tier 1 buff"));
                            args.Player.SetBuff(12, 14400);
                            args.Player.SendInfoMessage("You have been buffed with a Night Owl potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff1cd = Config.contents.buff1cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Swiftness
                case "swiftness":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff1cost;
                        Money moneyamount2 = Config.contents.buff1cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff1cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff1cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 1 buff.", moneyamount2, args.Player.Name), string.Format("Tier 1 buff"));
                            args.Player.SetBuff(3, 14400);
                            args.Player.SendInfoMessage("You have been buffed with a Swiftness potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff1cd = Config.contents.buff1cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Waterwalking
                case "waterwalking":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff1cost;
                        Money moneyamount2 = Config.contents.buff1cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff1cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff1cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 1 buff.", moneyamount2, args.Player.Name), string.Format("Tier 1 buff"));
                            args.Player.SetBuff(15, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Water Walking potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff1cd = Config.contents.buff1cd;
                            }
                        }
                    }
                    break;
                #endregion

                #endregion

                #region Regular player buffs 200TC

                #region Builder
                case "builder":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(107, 54000);
                            args.Player.SendInfoMessage("You have been buffed with a Builder potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Calming
                case "calming":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(106, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Calming potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Dangersense
                case "dangersense":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(111, 36000);
                            args.Player.SendInfoMessage("You have been buffed with a Dangersense potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion                

                #region Flipper
                case "flipper":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(109, 28800);
                            args.Player.SendInfoMessage("You have been buffed with a Flipper potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Gills
                case "gills":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(4, 7200);
                            args.Player.SendInfoMessage("You have been buffed with a Gills potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Warmth
                case "warmth":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(124, 54000);
                            args.Player.SendInfoMessage("You have been buffed with a Warmth potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Hunter
                case "hunter":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(17, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Hunter potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Heartreach
                case "heartreach":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(105, 28800);
                            args.Player.SendInfoMessage("You have been buffed with a Heartreach potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Gravitation
                case "gravitation":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff2cost;
                        Money moneyamount2 = Config.contents.buff2cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff2cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff2cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 2 buff.", moneyamount2, args.Player.Name), string.Format("Tier 2 buff"));
                            args.Player.SetBuff(18, 10800);
                            args.Player.SendInfoMessage("You have been buffed with a Gravitation potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff2cd = Config.contents.buff2cd;
                            }
                        }
                    }
                    break;
                #endregion

                #endregion

                #region Regular player buffs 350TC

                #region Shine            
                case "shine":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(11, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Shine potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Archery            
                case "archery":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(16, 14400);
                            args.Player.SendInfoMessage("You have been buffed with an Archery potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Ammores            
                case "ammores":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(112, 25200);
                            args.Player.SendInfoMessage("You have been buffed with an Ammo Reservation potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Featherfall            
                case "featherfall":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(8, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Featherfall potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Battle            
                case "battle":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(13, 25200);
                            args.Player.SendInfoMessage("You have been buffed with a Battle potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Mining            
                case "mining":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(104, 28800);
                            args.Player.SendInfoMessage("You have been buffed with a Mining potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Wrath            
                case "wrath":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(117, 14400);
                            args.Player.SendInfoMessage("You have been buffed with a Wrath potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Titan            
                case "titan":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(108, 14400);
                            args.Player.SendInfoMessage("You have been buffed with a Titan potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Thorns            
                case "thorns":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(14, 7200);
                            args.Player.SendInfoMessage("You have been buffed with a Thorns potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Summoning
                case "summoning":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(110, 21600);
                            args.Player.SendInfoMessage("You have been buffed with a Summoning potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Regeneration            
                case "regeneration":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(2, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Regeneration potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Rage            
                case "rage":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(115, 14400);
                            args.Player.SendInfoMessage("You have been buffed with a Rage potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Obsidian            
                case "obsidian":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(1, 14400);
                            args.Player.SendInfoMessage("You have been buffed with an Obsidian Skin potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Manareg            
                case "manareg":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(6, 25200);
                            args.Player.SendInfoMessage("You have been buffed with a Mana Regeneration potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Magicpower        
                case "magicpower":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(7, 7200);
                            args.Player.SendInfoMessage("You have been buffed with a Magic Power potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Lifeforce            
                case "lifeforce":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(113, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Lifeforce potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Ironskin            
                case "ironskin":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(5, 18000);
                            args.Player.SendInfoMessage("You have been buffed with an Ironskin potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Inferno            
                case "inferno":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(116, 14400);
                            args.Player.SendInfoMessage("You have been buffed with an Inferno potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #region Endurance            
                case "endurance":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff3cost;
                        Money moneyamount2 = Config.contents.buff3cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff3cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff3cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 3 buff.", moneyamount2, args.Player.Name), string.Format("Tier 3 buff"));
                            args.Player.SetBuff(114, 14400);
                            args.Player.SendInfoMessage("You have been buffed with an Endurance potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff3cd = Config.contents.buff3cd;
                            }
                        }
                    }
                    break;
                #endregion

                #endregion

                #region Regular player buffs 500TC

                #region Spelunker
                case "spelunker":
                    {
                        var Journalpayment = Wolfje.Plugins.SEconomy.Journal.BankAccountTransferOptions.AnnounceToSender;
                        var selectedPlayer = SEconomyPlugin.Instance.GetBankAccount(args.Player.User.Name);
                        var playeramount = selectedPlayer.Balance;
                        var player = Playerlist[args.Player.Index];
                        Money moneyamount = -Config.contents.buff4cost;
                        Money moneyamount2 = Config.contents.buff4cost;
                        if (playeramount < moneyamount2)
                        {
                            args.Player.SendErrorMessage("You need {0} to use this buff. You have {1}.", moneyamount2, selectedPlayer.Balance);
                            return;
                        }
                        if (player.buff4cd > 0)
                        {
                            args.Player.SendErrorMessage("This buff is on cooldown for {0} seconds.", player.buff4cd);
                            return;
                        }
                        else
                        {
                            SEconomyPlugin.Instance.WorldAccount.TransferToAsync(selectedPlayer, moneyamount, Journalpayment, string.Format("You paid {0} for a Tier 4 buff.", moneyamount2, args.Player.Name), string.Format("Tier 4 buff"));
                            args.Player.SetBuff(9, 18000);
                            args.Player.SendInfoMessage("You have been buffed with a Spelunker potion.");
                            if (!args.Player.Group.HasPermission("geldar.bypasscd"))
                            {
                                player.buff4cd = Config.contents.buff4cd;
                            }
                        }
                    }
                    break;
                    #endregion

                    #endregion
            }
        }
        #endregion

        #region Geldar
        private void Geldar(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Info: For the most basic commands use /geldar info.", Color.Goldenrod);
                args.Player.SendMessage("info: For rules use /geldar <general/chat/housing/itemdrop/further>", Color.Goldenrod);
                args.Player.SendMessage("Info: Be warned! The full list of rules can be found on our website www.geldar.net", Color.Goldenrod);
                args.Player.SendMessage("Info: The rules here are just the most important ones, shortened to fit.", Color.Goldenrod);
                args.Player.SendMessage("Info: If you lost your starter weapon, you can replace it with the command /starter <mage/warrior/ranger/summoner>", Color.Goldenrod);
                return;
            }

            switch (args.Parameters[0])
            {
                #region Info
                case "info":
                    {
                        args.Player.SendMessage("Welcome to our server, Geldar.", Color.Goldenrod);
                        args.Player.SendMessage("Info: You need level 10 for mining and level 20 to have a house.", Color.SkyBlue);
                        args.Player.SendMessage("Houses can be built above or under spawn.", Color.SkyBlue);
                        args.Player.SendMessage("Info: You can use /spawn to teleport to the map's spawnpoint.", Color.SkyBlue);
                        args.Player.SendMessage("Info: The server uses an ingame serverside currency name Terra Coins.", Color.SkyBlue);
                        args.Player.SendMessage("Info: You need these Terra Coins (tc) to level up, trade, or use ceratin commands.", Color.SkyBlue);
                        args.Player.SendMessage("Info: To check your tc balance use /bank bal or /bb. Earn tc by killing monsters.", Color.SkyBlue);
                        args.Player.SendMessage("Info: For tutorials please use /tutorial.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region General
                case "general":
                    {
                        args.Player.SendMessage("------------------------ General Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: On the Main map you are not allowed to build arenas on the surface.", Color.SkyBlue);
                        args.Player.SendMessage("Info: No massive terraforming. ( Destroying mountains, making skybridges, walls.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not obstruct players in free movement. (walls,barricades,holes)", Color.SkyBlue);
                        args.Player.SendMessage("Info: Afk farms are not allowed. (boxes around you, mob trapholes)", Color.SkyBlue);
                        args.Player.SendMessage("Info: Going afk while you are protected(gaining tc while afk), is not allowed.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Check the \"Is it Cheating\" thread on our forum.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Using any kind of bug/exploit/glitch will get you banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: We will ban for the smallest grief.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Using modified/hacked clients will get you banned permanently.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region Chat
                case "chat":
                    {
                        args.Player.SendMessage("------------------------ Chat Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: Write in English or you will get muted, kicked or banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Don't use offensive, allcaps spammy character names.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Spamming/flooding the chat will get you muted or banned.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Keep the swearing to a minimum.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Racist and discriminative comments will be harshly dealt with.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Advertising anything will get you banned.", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region Housing
                case "housing":
                    {
                        args.Player.SendMessage("------------------------ Housing Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: You can only have one house. All your characters must use the same one.", Color.SkyBlue);
                        args.Player.SendMessage("Info: House size limit is 15 blocks wide and 12 blocks high. Walls counted in.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Unprotected houses will be removed after 2 days.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not put spikes or anything else on your house that can obstruct players.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Only build houses abouve or under spawn where we marked spots.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Bigger clouds are for more than one player. Build on the side of the island.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Do not overlap houses, do not create one big house with your friends.", Color.SkyBlue);
                        args.Player.SendMessage("Info: Every house is limited to 5 chests. (Piggy banks, safes included)", Color.SkyBlue);
                        args.Player.SendMessage("Press enter to scroll the chat.", Color.Goldenrod);
                    }
                    break;
                #endregion

                #region Itemdrop
                case "itemdrop":
                    {
                        args.Player.SendMessage("------------------------ Item Drop Rules ------------------------", Color.Goldenrod);
                        args.Player.SendMessage("Info: You can't drop items on the server. Rebind your drop key.", Color.SkyBlue);
                        args.Player.SendMessage("Info: We can't help you with lost items.", Color.SkyBlue);
                    }
                    break;
                    #endregion
            }
        }
        #endregion

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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace ChaosMobs
{
    public static class Config
    {
        public static int ChaosChance = 5;
        public static int MinChaosMultiplier = 3;
        public static int MaxChaosMultiplier = 6;
        public static float FinalMultiplierForXpGain = 0.5f;

        public static List<int> IgnoredTypes = new List<int>()
        {
            NPCID.DungeonGuardian,
            NPCID.Bunny,
            NPCID.BunnySlimed,
            NPCID.BunnyXmas,
            NPCID.GoldBunny,
            NPCID.PartyBunny,
            NPCID.Penguin,
            NPCID.PenguinBlack,
            NPCID.Bird,
            NPCID.GoldBird,
            NPCID.ScorpionBlack,
            NPCID.Buggy,
            NPCID.Duck,
            NPCID.Duck2,
            NPCID.DuckWhite,
            NPCID.DuckWhite2,
            NPCID.Frog,
            NPCID.GoldFrog,
            NPCID.Worm,
            NPCID.GoldWorm,
            NPCID.TruffleWorm,
            NPCID.Goldfish,
            NPCID.GoldfishWalker,
            NPCID.Grasshopper,
            NPCID.GoldGrasshopper,
            NPCID.LightningBug,
            NPCID.Mouse,
            NPCID.GoldMouse,
            NPCID.Squirrel,
            NPCID.SquirrelGold,
            NPCID.SquirrelRed,
            NPCID.Scorpion,
            NPCID.Sluggy,
            NPCID.Snail,
            NPCID.GlowingSnail,
            NPCID.SeaSnail,
            NPCID.Butterfly,
            NPCID.GoldButterfly,
            NPCID.Firefly,
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsTail,
            NPCID.DevourerBody,
            NPCID.DevourerHead,
            NPCID.DevourerTail,
            NPCID.GiantWormBody,
            NPCID.GiantWormHead,
            NPCID.GiantWormTail,
            NPCID.DuneSplicerBody,
            NPCID.DuneSplicerHead,
            NPCID.DuneSplicerTail,
            NPCID.LeechBody,
            NPCID.LeechHead,
            NPCID.LeechTail,
            NPCID.StardustWormBody,
            NPCID.StardustWormHead,
            NPCID.StardustWormTail,
            NPCID.SolarCrawltipedeBody,
            NPCID.SolarCrawltipedeHead,
            NPCID.SolarCrawltipedeTail,
            NPCID.SeekerBody,
            NPCID.SeekerHead,
            NPCID.SeekerTail,
            NPCID.DiggerBody,
            NPCID.DiggerHead,
            NPCID.DiggerTail,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail,
            NPCID.WyvernBody,
            NPCID.WyvernBody2,
            NPCID.WyvernBody3,
            NPCID.WyvernHead,
            NPCID.WyvernLegs,
            NPCID.WyvernTail,
            NPCID.TombCrawlerBody,
            NPCID.TombCrawlerHead,
            NPCID.TombCrawlerTail,
            NPCID.BoneSerpentBody,
            NPCID.BoneSerpentHead,
            NPCID.BoneSerpentTail,
            NPCID.PrimeCannon,
            NPCID.PrimeLaser,
            NPCID.PrimeSaw,
            NPCID.PrimeVice,
            NPCID.CultistDragonBody1,
            NPCID.CultistDragonBody2,
            NPCID.CultistDragonBody3,
            NPCID.CultistDragonBody4,
            NPCID.CultistDragonHead,
            NPCID.CultistDragonTail,
            NPCID.GolemHead,
            NPCID.GolemHeadFree,
            NPCID.MoonLordHead,
            NPCID.SkeletronHead
        };

        public static bool IsIgnoredType(NPC npc)
        {
            return IgnoredTypes.Contains(npc.type) ||
                    npc.TypeName.ToLower().Contains("head") ||
                    npc.TypeName.ToLower().Contains("body") ||
                    npc.TypeName.ToLower().Contains("tail") ||
                    npc.TypeName.ToLower().Contains("pillar") ||
                    npc.FullName.ToLower().Contains("head") ||
                    npc.FullName.ToLower().Contains("body") ||
                    npc.FullName.ToLower().Contains("tail") ||
                    npc.FullName.ToLower().Contains("pillar") ||
                    npc.GivenName.ToLower().Contains("head") ||
                    npc.GivenName.ToLower().Contains("body") ||
                    npc.GivenName.ToLower().Contains("tail") ||
                    npc.GivenName.ToLower().Contains("pillar") ||
                    npc.aiStyle == 6;
        }

        private static readonly string CommonConfigPath = Path.Combine(Main.SavePath, "Mod Configs/ChaosMobs", "ChaosMobs_Common.json");
        private static readonly string IgnoresConfigPath = Path.Combine(Main.SavePath, "Mod Configs/ChaosMobs", "ChaosMobs_Ignores.json");

        private static readonly Preferences CommonConfig = new Preferences(CommonConfigPath);
        private static readonly Preferences IgnoresConfig = new Preferences(IgnoresConfigPath);

        private static void CreateConfig(Preferences conf)
        {
            conf.Clear();

            if (conf == CommonConfig)
            {
                conf.Put("ChaosChance", ChaosChance);
                conf.Put("MinChaosMultiplier", MinChaosMultiplier);
                conf.Put("MaxChaosMultiplier", MaxChaosMultiplier);
                conf.Put("FinalMultiplierForXpGain", FinalMultiplierForXpGain);
            }
            else if (conf == IgnoresConfig)
            {
                conf.Put("IgnoredTypes", IgnoredTypes);
            }
        }

        public static void Load()
        {
            if (!ReadConfig(CommonConfig))
            {
                ErrorLogger.Log("Failed to read config file: ChaosMobs_Common.json! Recreating config...");
                CreateConfig(CommonConfig);
            }
            if (!ReadConfig(IgnoresConfig))
            {
                ErrorLogger.Log("Failed to read config file: ChaosMobs_Ignores.json! Recreating config...");
                CreateConfig(IgnoresConfig);
            }
        }

        private static bool ReadConfig(Preferences conf)
        {
            if (conf.Load())
            {
                if (conf == CommonConfig)
                {
                    conf.Get("ChaosChance", ref ChaosChance);
                    conf.Get("MinChaosMultiplier", ref MinChaosMultiplier);
                    conf.Get("MaxChaosMultiplier", ref MaxChaosMultiplier);
                    conf.Get("FinalMultiplierForXpGain", ref FinalMultiplierForXpGain);
                }
                else if (conf == IgnoresConfig)
                {
                    conf.Get("IgnoredTypes", ref IgnoredTypes);
                }
                return true;
            }
            return false;
        }
    }
}

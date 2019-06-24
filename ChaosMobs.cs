using System;
using Terraria.ModLoader;

namespace ChaosMobs
{
	class ChaosMobs : Mod
	{
        public static string[] MinorStats =
{
            "Melee Damage",
            "Melee Crit",
            "Melee Speed",
            "Ranged Damage",
            "Ranged Crit",
            "Magic Damage",
            "Magic Crit",
            "Minion Damage",
            "Max Minions",
            "Max Run Speed",
            "Dodge Chance",
            "Block Chance"
        };

        public const int MaxLevel = 200; // Self-explanatory
        public int[] XpNeededForChaosRank { get; private set; }

        public ChaosMobs()
		{
		}

        public override void Load()
        {
            Config.Load();

            XpNeededForChaosRank = new int[MaxLevel + 1];

            XpNeededForChaosRank[0] = 0;
            XpNeededForChaosRank[1] = 40;

            for (int i = 2; i < XpNeededForChaosRank.Length; i++)
            {
                double value = 2 * (12 * Math.Pow(i, 2) + 1.486 * i * Math.Pow(i, 1.6 * Math.Sqrt(1 - 1 / i)) * Math.Log(i)) + XpNeededForChaosRank[i - 1];
                XpNeededForChaosRank[i] = (int)(value / 1.5f);
            }
        }
    }
}

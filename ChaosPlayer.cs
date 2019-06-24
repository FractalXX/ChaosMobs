using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaosMobs
{
    class ChaosPlayer : ModPlayer
    {
        public static Random random = new Random();

        public IDictionary<string, float> ChaosBonuses { get; private set; }

        public int chaosRank;
        public long chaosXp;
        public int chaosPoints;

        public override void Initialize()
        {
            this.ChaosBonuses = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

            foreach (string stat in ChaosMobs.MinorStats)
            {
                this.ChaosBonuses[stat] = 0;
            }
        }

        /// <summary>
        /// Raises the player's chaos rank by one. (with effect)
        /// </summary>
        public void ChaosRankUp()
        {
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDust(this.player.position, random.Next(5, 15), random.Next(5, 15), 179, random.Next(-10, 10), random.Next(-10, 10));
            }
            CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 100, 100), Color.Violet, "Chaos Rank Up");

            this.chaosPoints++;

            this.chaosRank++;
            if (Main.netMode == 0) Main.NewText(String.Format("You have reached chaos rank {0}!", this.chaosRank), 179, 104, 255);
            else NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(String.Format("{0} has reached chaos rank {1}!", this.player.name, this.chaosRank)), 179, 104, 255, 0, 0);
        }

        public void GainExperience(int value)
        {
            ChaosMobs mod = this.mod as ChaosMobs;

            if (this.chaosXp < mod.XpNeededForChaosRank[ChaosMobs.MaxLevel])
            {
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 50, 50), Color.DeepPink, String.Format("+{0} Chaos XP", value));
                this.chaosXp += value;
            }
        }

        public override void PostUpdate()
        {
            ChaosMobs mod = this.mod as ChaosMobs;

            if (this.chaosXp > mod.XpNeededForChaosRank[ChaosMobs.MaxLevel])
            {
                this.chaosXp = mod.XpNeededForChaosRank[ChaosMobs.MaxLevel];
            }

            if (this.chaosRank < ChaosMobs.MaxLevel && this.chaosXp >= mod.XpNeededForChaosRank[this.chaosRank + 1])
            {
                this.ChaosRankUp();
            }
        }

        public override void ResetEffects()
        {
            this.player.meleeDamage += this.ChaosBonuses["Melee Damage"];
            this.player.rangedDamage += this.ChaosBonuses["Ranged Damage"];
            this.player.magicDamage += this.ChaosBonuses["Magic Damage"];

            this.player.meleeCrit += (int)this.ChaosBonuses["Melee Crit"];
            this.player.rangedCrit += (int)this.ChaosBonuses["Ranged Crit"];
            this.player.magicCrit += (int)this.ChaosBonuses["Magic Crit"];

            this.player.minionDamage += this.ChaosBonuses["Minion Damage"];
            this.player.maxMinions += (int)this.ChaosBonuses["Max Minions"];
            this.player.maxTurrets += (int)Math.Floor(this.ChaosBonuses["Max Minions"] / 2);

            this.player.meleeSpeed += this.ChaosBonuses["Melee Speed"];
            this.player.maxRunSpeed += this.ChaosBonuses["Max Run Speed"] * 3;
        }
    }
}

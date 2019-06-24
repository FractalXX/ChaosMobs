using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaosMobs
{
    class ChaosGlobalNpc : GlobalNPC
    {
        /// <summary>
        /// The override color for chaos mobs.
        /// </summary>
        public static Color ChaosColor = new Color(179, 104, 255, 127);

        private static Random random = new Random();

        /// <summary>
        /// Returns true if this NPC is a chaos NPC.
        /// </summary>
        public bool isChaos = false;

        /// <summary>
        /// Determines the stat scale of the NPC if it's a chaos NPC.
        /// </summary>
        public int chaosMultiplier = 1;

        /// <summary>
        /// Stores references to players who damaged this NPC.
        /// </summary>
        private List<ChaosPlayer> attackingPlayers { get; set; }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public ChaosGlobalNpc()
        {
            this.attackingPlayers = new List<ChaosPlayer>();
        }

        public void ChaosTransform(NPC npc)
        {
            this.chaosMultiplier = random.Next(Config.MinChaosMultiplier, Config.MaxChaosMultiplier);

            npc.scale *= this.chaosMultiplier / 2.7f;
            npc.lifeMax *= this.chaosMultiplier;
            npc.life = npc.lifeMax;
            npc.defDamage *= this.chaosMultiplier;
            npc.defDefense *= this.chaosMultiplier / 2;
            npc.color = ChaosColor;
            npc.stepSpeed *= this.chaosMultiplier / 2f;

            this.isChaos = true;
        }

        public override bool CheckDead(NPC npc)
        {
            if (!Config.IsIgnoredType(npc) && !npc.SpawnedFromStatue && !npc.friendly)
            {
                if (npc.lifeMax >= 10)
                {
                    double gainedXp;
                    foreach (ChaosPlayer modPlayer in this.attackingPlayers)
                    {
                            gainedXp = Math.Pow(2, Math.Sqrt((2 * (1 + npc.defDamage / (2 * npc.lifeMax)) * npc.lifeMax) / Math.Pow(npc.lifeMax, 1 / 2.6)));
                            if (npc.lifeMax >= 1000)
                            {
                                gainedXp = npc.lifeMax / 2;
                            }
                            else if (npc.lifeMax <= 20)
                            {
                                gainedXp /= 2;
                            }
                            modPlayer.GainExperience((int)((1 + (gainedXp / 3)) * Config.FinalMultiplierForXpGain));
                    }
                }
            }
            return base.CheckDead(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            if (this.isChaos)
            {
                npc.GivenName = String.Format("Chaos {0}", npc.TypeName);
            }
        }

        public override void SetDefaults(NPC npc)
        {
            // Fix for incompatibility with other mods such as Calamity, etc.
            if (npc != null && Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
            {
                if (!npc.boss && npc.lifeMax >= 40 && !npc.SpawnedFromStatue && !npc.friendly && !Config.IsIgnoredType(npc) && random.Next(0, 101) <= Config.ChaosChance)
                {
                    ChaosTransform(npc);
                }
            }
        }

        public void SubscribePlayer(ChaosPlayer player)
        {
            if (!this.attackingPlayers.Contains(player))
            {
                this.attackingPlayers.Add(player);
            }
        }
    }
}

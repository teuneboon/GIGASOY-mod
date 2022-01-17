using Terraria;
using Terraria.ModLoader;

namespace GIGASOY.Buffs
{
    public class SoyBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Defensive Nerf");
            Description.SetDefault("Grants -40 defense.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 40; //Grant a -40 defense boost to the player while the buff is active.
        }
    }
}
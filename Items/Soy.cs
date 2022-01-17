using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GIGASOY.Items
{
	public class Soy : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("SoySword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("This item gives you all the benefits of soy.");
		}

		public override void SetDefaults()
		{
            item.width = 28;
            item.height = 28;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.Orange;
            item.value = Item.buyPrice(copper: 1);
            item.buffType = ModContent.BuffType<Buffs.SoyBuff>(); //Specify an existing buff to be applied when used.
            item.buffTime = 5400; //The amount of time the buff declared in item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
        }
	}
}
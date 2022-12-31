using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Guide.Content.Buffs
{
	public class BurningLow: ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("[c/ff0000:Low Flames]");
			Description.SetDefault("[c/ffcc33:The embers that make up your existence are trying to reignite.]");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed -= 0.2f;
		}
	}
}
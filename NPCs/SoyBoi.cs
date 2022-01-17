using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GIGASOY.NPCs
{
	// This ModNPC serves as an example of a complete AI example.
	public class SoyBoi : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soy Boi");
			Main.npcFrameCount[npc.type] = 1; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 56;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			npc.damage = 7;
			npc.defense = 20;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			//npc.alpha = 175;
			//npc.color = new Color(0, 80, 255, 100);
			npc.value = 25f;
			npc.buffImmune[BuffID.Confused] = false; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
		}

		public override float SpawnChance(NPCSpawnInfo info)
		{
			// we would like this npc to spawn in the overworld.
			return info.spawnTileY <= Main.worldSurface ? 0.1f : 0f;
		}

		// These const ints are for the benefit of the programmer. Organization is key to making an AI that behaves properly without driving you crazy.
		// Here I lay out what I will use each of the 4 npc.ai slots for.
		private const int AI_State_Slot = 0;
		private const int AI_Timer_Slot = 1;
		private const int AI_Unused_Slot_2 = 2;
		private const int AI_Unused_Slot_3 = 3;

		// npc.localAI will also have 4 float variables available to use. With ModNPC, using just a local class member variable would have the same effect.
		private const int Local_AI_Unused_Slot_0 = 0;
		private const int Local_AI_Unused_Slot_1 = 1;
		private const int Local_AI_Unused_Slot_2 = 2;
		private const int Local_AI_Unused_Slot_3 = 3;

		// Here I define some values I will use with the State slot. Using an ai slot as a means to store "state" can simplify things greatly. Think flowchart.
		private const int State_Asleep = 0;
		private const int State_Notice = 1;
		private const int State_Flee = 2;

		// This is a property (https://msdn.microsoft.com/en-us/library/x9fsa0sw.aspx), it is very useful and helps keep out AI code clear of clutter.
		// Without it, every instance of "AI_State" in the AI code below would be "npc.ai[AI_State_Slot]". 
		// Also note that without the "AI_State_Slot" defined above, this would be "npc.ai[0]".
		// This is all to just make beautiful, manageable, and clean code.
		public float AI_State
		{
			get => npc.ai[AI_State_Slot];
			set => npc.ai[AI_State_Slot] = value;
		}

		public float AI_Timer
		{
			get => npc.ai[AI_Timer_Slot];
			set => npc.ai[AI_Timer_Slot] = value;
		}

		// AdvancedFlutterSlime will need: float in water, diminishing aggo, spawn projectiles.

		// Our AI here makes our NPC sit waiting for a player to enter range, jumps to attack, flutter mid-fall to stay afloat a little longer, then falls to the ground. Note that animation should happen in FindFrame
		public override void AI()
		{
			// The npc starts in the asleep state, waiting for a player to enter range
			if (AI_State == State_Asleep)
			{
				// TargetClosest sets npc.target to the player.whoAmI of the closest player. the faceTarget parameter means that npc.direction will automatically be 1 or -1 if the targeted player is to the right or left. This is also automatically flipped if npc.confused
				npc.TargetClosest(true);
				// Now we check the make sure the target is still valid and within our specified notice range (500)
				if (npc.HasValidTarget && Main.player[npc.target].Distance(npc.Center) < 500f)
				{
					// Since we have a target in range, we change to the Notice state. (and zero out the Timer for good measure)
					AI_State = State_Notice;
					AI_Timer = 0;
				}
			}
			// In this state, a player has been targeted
			else if (AI_State == State_Notice)
			{
				// If the targeted player is in attack range (250).
				if (Main.player[npc.target].Distance(npc.Center) < 250f)
				{
					// Here we use our Timer to wait .33 seconds before actually jumping. In FindFrame you'll notice AI_Timer also being used to animate the pre-jump crouch
					AI_Timer++;
					if (AI_Timer >= 20)
					{
						AI_State = State_Flee;
						AI_Timer = 0;
					}
				}
				else
				{
					npc.TargetClosest(true);
					if (!npc.HasValidTarget || Main.player[npc.target].Distance(npc.Center) > 500f)
					{
						// Out targeted player seems to have left our range, so we'll go back to sleep.
						AI_State = State_Asleep;
						AI_Timer = 0;
					}
				}
			}
			// In this state, we are in the jump. 
			else if (AI_State == State_Flee)
			{
				AI_Timer++;
				if (AI_Timer == 1)
				{
					// We apply an initial velocity the first tick we are in the Jump frame. Remember that -Y is up. 
					npc.velocity = new Vector2(npc.direction * -6, -25f);
				}
				else if (AI_Timer >= 40)
				{
					AI_State = State_Asleep;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
		}

		public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Soy>());
		}
	}
}
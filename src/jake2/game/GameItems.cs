using J2N.Text;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Game
{
	public class GameItems
	{
		public static gitem_armor_t jacketarmor_info = new gitem_armor_t( 25, 50, 0.3F, 0F, Defines.ARMOR_JACKET );
		public static gitem_armor_t combatarmor_info = new gitem_armor_t( 50, 100, 0.6F, 0.3F, Defines.ARMOR_COMBAT );
		public static gitem_armor_t bodyarmor_info = new gitem_armor_t( 100, 200, 0.8F, 0.6F, Defines.ARMOR_BODY );
		static Int32 quad_drop_timeout_hack = 0;
		static Int32 jacket_armor_index;
		static Int32 combat_armor_index;
		static Int32 body_armor_index;
		static Int32 power_screen_index;
		static Int32 power_shield_index;
		static EntThinkAdapter DoRespawn = new AnonymousEntThinkAdapter();
		private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "do_respawn";
			}

			public override Boolean Think( edict_t ent )
			{
				if ( ent.team != null )
				{
					edict_t master;
					var count = 0;
					var choice = 0;
					master = ent.teammaster;
					choice = Lib.Rand() % count;
				}

				ent.svflags &= ~Defines.SVF_NOCLIENT;
				ent.solid = Defines.SOLID_TRIGGER;
				GameBase.gi.Linkentity( ent );
				ent.s.event_renamed = Defines.EV_ITEM_RESPAWN;
				return false;
			}
		}

		public static EntInteractAdapter Pickup_Pack = new AnonymousEntInteractAdapter();
		private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_pack";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				gitem_t item;
				Int32 index;
				if ( other.client.pers.max_bullets < 300 )
					other.client.pers.max_bullets = 300;
				if ( other.client.pers.max_shells < 200 )
					other.client.pers.max_shells = 200;
				if ( other.client.pers.max_rockets < 100 )
					other.client.pers.max_rockets = 100;
				if ( other.client.pers.max_grenades < 100 )
					other.client.pers.max_grenades = 100;
				if ( other.client.pers.max_cells < 300 )
					other.client.pers.max_cells = 300;
				if ( other.client.pers.max_slugs < 100 )
					other.client.pers.max_slugs = 100;
				item = FindItem( "Bullets" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_bullets )
						other.client.pers.inventory[index] = other.client.pers.max_bullets;
				}

				item = FindItem( "Shells" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_shells )
						other.client.pers.inventory[index] = other.client.pers.max_shells;
				}

				item = FindItem( "Cells" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_cells )
						other.client.pers.inventory[index] = other.client.pers.max_cells;
				}

				item = FindItem( "Grenades" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_grenades )
						other.client.pers.inventory[index] = other.client.pers.max_grenades;
				}

				item = FindItem( "Rockets" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_rockets )
						other.client.pers.inventory[index] = other.client.pers.max_rockets;
				}

				item = FindItem( "Slugs" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_slugs )
						other.client.pers.inventory[index] = other.client.pers.max_slugs;
				}

				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, ent.item.quantity );
				return true;
			}
		}

		public static readonly EntInteractAdapter Pickup_Health = new AnonymousEntInteractAdapter1();
		private sealed class AnonymousEntInteractAdapter1 : EntInteractAdapter
		{
			public override String GetID( )
			{
				return "pickup_health";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				if ( 0 == ( ent.style & Defines.HEALTH_IGNORE_MAX ) )
					if ( other.health >= other.max_health )
						return false;
				other.health += ent.count;
				if ( 0 == ( ent.style & Defines.HEALTH_IGNORE_MAX ) )
				{
					if ( other.health > other.max_health )
						other.health = other.max_health;
				}

				if ( 0 != ( ent.style & Defines.HEALTH_TIMED ) )
				{
					ent.think = GameUtil.MegaHealth_think;
					ent.nextthink = GameBase.level.time + 5F;
					ent.owner = other;
					ent.flags |= Defines.FL_RESPAWN;
					ent.svflags |= Defines.SVF_NOCLIENT;
					ent.solid = Defines.SOLID_NOT;
				}
				else
				{
					if ( !( ( ent.spawnflags & Defines.DROPPED_ITEM ) != 0 ) && ( GameBase.deathmatch.value != 0 ) )
						SetRespawn( ent, 30 );
				}

				return true;
			}
		}

		public static EntTouchAdapter Touch_Item_Cmd = new AnonymousEntTouchAdapter();
		private sealed class AnonymousEntTouchAdapter : EntTouchAdapter
		{

			public override String GetID( )
			{
				return "touch_item";
			}

			public override void Touch( edict_t ent, edict_t other, cplane_t plane, csurface_t surf )
			{
				Boolean taken;
				if ( ent.classname.Equals( "item_breather" ) )
					taken = false;
				if ( other.client == null )
					return;
				if ( other.health < 1 )
					return;
				if ( ent.item.pickup == null )
					return;
				taken = ent.item.pickup.Interact( ent, other );
				if ( taken )
				{
					other.client.bonus_alpha = 0.25F;
					other.client.ps.stats[Defines.STAT_PICKUP_ICON] = ( Int16 ) GameBase.gi.Imageindex( ent.item.icon );
					other.client.ps.stats[Defines.STAT_PICKUP_STRING] = ( Int16 ) ( Defines.CS_ITEMS + ITEM_INDEX( ent.item ) );
					other.client.pickup_msg_time = GameBase.level.time + 3F;
					if ( ent.item.use != null )
						other.client.pers.selected_item = other.client.ps.stats[Defines.STAT_SELECTED_ITEM] = ( Int16 ) ITEM_INDEX( ent.item );
					if ( ent.item.pickup == Pickup_Health )
					{
						if ( ent.count == 2 )
							GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/s_health.wav" ), 1, Defines.ATTN_NORM, 0 );
						else if ( ent.count == 10 )
							GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/n_health.wav" ), 1, Defines.ATTN_NORM, 0 );
						else if ( ent.count == 25 )
							GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/l_health.wav" ), 1, Defines.ATTN_NORM, 0 );
						else
							GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/m_health.wav" ), 1, Defines.ATTN_NORM, 0 );
					}
					else if ( ent.item.pickup_sound != null )
					{
						GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( ent.item.pickup_sound ), 1, Defines.ATTN_NORM, 0 );
					}
				}

				if ( 0 == ( ent.spawnflags & Defines.ITEM_TARGETS_USED ) )
				{
					GameUtil.G_UseTargets( ent, other );
					ent.spawnflags |= Defines.ITEM_TARGETS_USED;
				}

				if ( !taken )
					return;
				Com.Dprintln( "Picked up:" + ent.classname );
				if ( !( ( GameBase.coop.value != 0 ) && ( ent.item.flags & Defines.IT_STAY_COOP ) != 0 ) || 0 != ( ent.spawnflags & ( Defines.DROPPED_ITEM | Defines.DROPPED_PLAYER_ITEM ) ) )
				{
					if ( ( ent.flags & Defines.FL_RESPAWN ) != 0 )
						ent.flags = ent.flags & ~Defines.FL_RESPAWN;
					else
						GameUtil.G_FreeEdict( ent );
				}
			}
		}

		static EntTouchAdapter drop_temp_touch = new AnonymousEntTouchAdapter1();
		private sealed class AnonymousEntTouchAdapter1 : EntTouchAdapter
		{

			public override String GetID( )
			{
				return "drop_temp_touch";
			}

			public override void Touch( edict_t ent, edict_t other, cplane_t plane, csurface_t surf )
			{
				if ( other == ent.owner )
					return;
				Touch_Item_Cmd.Touch( ent, other, plane, surf );
			}
		}

		static EntThinkAdapter drop_make_touchable = new AnonymousEntThinkAdapter1();
		private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "drop_make_touchable";
			}

			public override Boolean Think( edict_t ent )
			{
				ent.touch = Touch_Item_Cmd;
				if ( GameBase.deathmatch.value != 0 )
				{
					ent.nextthink = GameBase.level.time + 29;
					ent.think = GameUtil.G_FreeEdictA;
				}

				return false;
			}
		}

		public static ItemUseAdapter Use_Quad = new AnonymousItemUseAdapter();
		private sealed class AnonymousItemUseAdapter : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_quad";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				Int32 timeout;
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				GameUtil.ValidateSelectedItem( ent );
				if ( quad_drop_timeout_hack != 0 )
				{
					timeout = quad_drop_timeout_hack;
					quad_drop_timeout_hack = 0;
				}
				else
				{
					timeout = 300;
				}

				if ( ent.client.quad_framenum > GameBase.level.framenum )
					ent.client.quad_framenum += timeout;
				else
					ent.client.quad_framenum = GameBase.level.framenum + timeout;
				GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/damage.wav" ), 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static ItemUseAdapter Use_Invulnerability = new AnonymousItemUseAdapter1();
		private sealed class AnonymousItemUseAdapter1 : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_invulnerability";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				GameUtil.ValidateSelectedItem( ent );
				if ( ent.client.invincible_framenum > GameBase.level.framenum )
					ent.client.invincible_framenum += 300;
				else
					ent.client.invincible_framenum = GameBase.level.framenum + 300;
				GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/protect.wav" ), 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static ItemUseAdapter Use_Breather = new AnonymousItemUseAdapter2();
		private sealed class AnonymousItemUseAdapter2 : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_breather";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				GameUtil.ValidateSelectedItem( ent );
				if ( ent.client.breather_framenum > GameBase.level.framenum )
					ent.client.breather_framenum += 300;
				else
					ent.client.breather_framenum = GameBase.level.framenum + 300;
				GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/damage.wav" ), 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static ItemUseAdapter Use_Envirosuit = new AnonymousItemUseAdapter3();
		private sealed class AnonymousItemUseAdapter3 : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_envirosuit";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				GameUtil.ValidateSelectedItem( ent );
				if ( ent.client.enviro_framenum > GameBase.level.framenum )
					ent.client.enviro_framenum += 300;
				else
					ent.client.enviro_framenum = GameBase.level.framenum + 300;
				GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/damage.wav" ), 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static ItemUseAdapter Use_Silencer = new AnonymousItemUseAdapter4();
		private sealed class AnonymousItemUseAdapter4 : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_silencer";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				GameUtil.ValidateSelectedItem( ent );
				ent.client.silencer_shots += 30;
				GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/damage.wav" ), 1, Defines.ATTN_NORM, 0 );
			}
		}

		public static EntInteractAdapter Pickup_Key = new AnonymousEntInteractAdapter2();
		private sealed class AnonymousEntInteractAdapter2 : EntInteractAdapter
		{
			public override String GetID( )
			{
				return "pickup_key";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				if ( GameBase.coop.value != 0 )
				{
					if ( Lib.Strcmp( ent.classname, "key_power_cube" ) == 0 )
					{
						if ( ( other.client.pers.power_cubes & ( ( ent.spawnflags & 0x0000ff00 ) >> 8 ) ) != 0 )
							return false;
						other.client.pers.inventory[ITEM_INDEX( ent.item )]++;
						other.client.pers.power_cubes |= ( ( ent.spawnflags & 0x0000ff00 ) >> 8 );
					}
					else
					{
						if ( other.client.pers.inventory[ITEM_INDEX( ent.item )] != 0 )
							return false;
						other.client.pers.inventory[ITEM_INDEX( ent.item )] = 1;
					}

					return true;
				}

				other.client.pers.inventory[ITEM_INDEX( ent.item )]++;
				return true;
			}
		}

		public static EntInteractAdapter Pickup_Ammo = new AnonymousEntInteractAdapter3();
		private sealed class AnonymousEntInteractAdapter3 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_ammo";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				Int32 oldcount;
				Int32 count;
				Boolean weapon;
				weapon = ( ent.item.flags & Defines.IT_WEAPON ) != 0;
				if ( ( weapon ) && ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) != 0 )
					count = 1000;
				else if ( ent.count != 0 )
					count = ent.count;
				else
					count = ent.item.quantity;
				oldcount = other.client.pers.inventory[ITEM_INDEX( ent.item )];
				if ( !Add_Ammo( other, ent.item, count ) )
					return false;
				if ( weapon && 0 == oldcount )
				{
					if ( other.client.pers.weapon != ent.item && ( 0 == GameBase.deathmatch.value || other.client.pers.weapon == FindItem( "blaster" ) ) )
						other.client.newweapon = ent.item;
				}

				if ( 0 == ( ent.spawnflags & ( Defines.DROPPED_ITEM | Defines.DROPPED_PLAYER_ITEM ) ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, 30 );
				return true;
			}
		}

		public static EntInteractAdapter Pickup_Armor = new AnonymousEntInteractAdapter4();
		private sealed class AnonymousEntInteractAdapter4 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_armor";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				Int32 old_armor_index;
				gitem_armor_t oldinfo;
				gitem_armor_t newinfo;
				Int32 newcount;
				Single salvage;
				Int32 salvagecount;
				newinfo = ( gitem_armor_t ) ent.item.info;
				old_armor_index = ArmorIndex( other );
				if ( ent.item.tag == Defines.ARMOR_SHARD )
				{
					if ( 0 == old_armor_index )
						other.client.pers.inventory[jacket_armor_index] = 2;
					else
						other.client.pers.inventory[old_armor_index] += 2;
				}
				else if ( 0 == old_armor_index )
				{
					other.client.pers.inventory[ITEM_INDEX( ent.item )] = newinfo.base_count;
				}
				else
				{
					if ( old_armor_index == jacket_armor_index )
						oldinfo = jacketarmor_info;
					else if ( old_armor_index == combat_armor_index )
						oldinfo = combatarmor_info;
					else
						oldinfo = bodyarmor_info;
					if ( newinfo.normal_protection > oldinfo.normal_protection )
					{
						salvage = oldinfo.normal_protection / newinfo.normal_protection;
						salvagecount = ( Int32 ) salvage * other.client.pers.inventory[old_armor_index];
						newcount = newinfo.base_count + salvagecount;
						if ( newcount > newinfo.max_count )
							newcount = newinfo.max_count;
						other.client.pers.inventory[old_armor_index] = 0;
						other.client.pers.inventory[ITEM_INDEX( ent.item )] = newcount;
					}
					else
					{
						salvage = newinfo.normal_protection / oldinfo.normal_protection;
						salvagecount = ( Int32 ) salvage * newinfo.base_count;
						newcount = other.client.pers.inventory[old_armor_index] + salvagecount;
						if ( newcount > oldinfo.max_count )
							newcount = oldinfo.max_count;
						if ( other.client.pers.inventory[old_armor_index] >= newcount )
							return false;
						other.client.pers.inventory[old_armor_index] = newcount;
					}
				}

				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, 20 );
				return true;
			}
		}

		public static EntInteractAdapter Pickup_PowerArmor_Cmd = new AnonymousEntInteractAdapter5();
		private sealed class AnonymousEntInteractAdapter5 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_powerarmor";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				Int32 quantity;
				quantity = other.client.pers.inventory[ITEM_INDEX( ent.item )];
				other.client.pers.inventory[ITEM_INDEX( ent.item )]++;
				if ( GameBase.deathmatch.value != 0 )
				{
					if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) )
						SetRespawn( ent, ent.item.quantity );
					if ( 0 == quantity )
						ent.item.use.Use( other, ent.item );
				}

				return true;
			}
		}

		public static EntInteractAdapter Pickup_Powerup = new AnonymousEntInteractAdapter6();
		private sealed class AnonymousEntInteractAdapter6 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_powerup";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				Int32 quantity;
				quantity = other.client.pers.inventory[ITEM_INDEX( ent.item )];
				if ( ( GameBase.skill.value == 1 && quantity >= 2 ) || ( GameBase.skill.value >= 2 && quantity >= 1 ) )
					return false;
				if ( ( GameBase.coop.value != 0 ) && ( ent.item.flags & Defines.IT_STAY_COOP ) != 0 && ( quantity > 0 ) )
					return false;
				other.client.pers.inventory[ITEM_INDEX( ent.item )]++;
				if ( GameBase.deathmatch.value != 0 )
				{
					if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) )
						SetRespawn( ent, ent.item.quantity );
					if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INSTANT_ITEMS ) != 0 || ( ( ent.item.use == Use_Quad ) && 0 != ( ent.spawnflags & Defines.DROPPED_PLAYER_ITEM ) ) )
					{
						if ( ( ent.item.use == Use_Quad ) && 0 != ( ent.spawnflags & Defines.DROPPED_PLAYER_ITEM ) )
							quad_drop_timeout_hack = ( Int32 ) ( ( ent.nextthink - GameBase.level.time ) / Defines.FRAMETIME );
						ent.item.use.Use( other, ent.item );
					}
				}

				return true;
			}
		}

		public static EntInteractAdapter Pickup_Adrenaline = new AnonymousEntInteractAdapter7();
		private sealed class AnonymousEntInteractAdapter7 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_adrenaline";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				if ( GameBase.deathmatch.value == 0 )
					other.max_health += 1;
				if ( other.health < other.max_health )
					other.health = other.max_health;
				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, ent.item.quantity );
				return true;
			}
		}

		public static EntInteractAdapter Pickup_AncientHead = new AnonymousEntInteractAdapter8();
		private sealed class AnonymousEntInteractAdapter8 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_ancienthead";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				other.max_health += 2;
				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, ent.item.quantity );
				return true;
			}
		}

		public static EntInteractAdapter Pickup_Bandolier = new AnonymousEntInteractAdapter9();
		private sealed class AnonymousEntInteractAdapter9 : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "pickup_bandolier";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				gitem_t item;
				Int32 index;
				if ( other.client.pers.max_bullets < 250 )
					other.client.pers.max_bullets = 250;
				if ( other.client.pers.max_shells < 150 )
					other.client.pers.max_shells = 150;
				if ( other.client.pers.max_cells < 250 )
					other.client.pers.max_cells = 250;
				if ( other.client.pers.max_slugs < 75 )
					other.client.pers.max_slugs = 75;
				item = FindItem( "Bullets" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_bullets )
						other.client.pers.inventory[index] = other.client.pers.max_bullets;
				}

				item = FindItem( "Shells" );
				if ( item != null )
				{
					index = ITEM_INDEX( item );
					other.client.pers.inventory[index] += item.quantity;
					if ( other.client.pers.inventory[index] > other.client.pers.max_shells )
						other.client.pers.inventory[index] = other.client.pers.max_shells;
				}

				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) && ( GameBase.deathmatch.value != 0 ) )
					SetRespawn( ent, ent.item.quantity );
				return true;
			}
		}

		public static ItemDropAdapter Drop_Ammo = new AnonymousItemDropAdapter();
		private sealed class AnonymousItemDropAdapter : ItemDropAdapter
		{
			public override String GetID( )
			{
				return "drop_ammo";
			}

			public override void Drop( edict_t ent, gitem_t item )
			{
				edict_t dropped;
				Int32 index;
				index = ITEM_INDEX( item );
				dropped = Drop_Item( ent, item );
				if ( ent.client.pers.inventory[index] >= item.quantity )
					dropped.count = item.quantity;
				else
					dropped.count = ent.client.pers.inventory[index];
				if ( ent.client.pers.weapon != null && ent.client.pers.weapon.tag == Defines.AMMO_GRENADES && item.tag == Defines.AMMO_GRENADES && ent.client.pers.inventory[index] - dropped.count <= 0 )
				{
					GameBase.gi.Cprintf( ent, Defines.PRINT_HIGH, "Can't drop current weapon\\n" );
					GameUtil.G_FreeEdict( dropped );
					return;
				}

				ent.client.pers.inventory[index] -= dropped.count;
				Cmd.ValidateSelectedItem( ent );
			}
		}

		public static ItemDropAdapter Drop_General = new AnonymousItemDropAdapter1();
		private sealed class AnonymousItemDropAdapter1 : ItemDropAdapter
		{
			public override String GetID( )
			{
				return "drop_general";
			}

			public override void Drop( edict_t ent, gitem_t item )
			{
				Drop_Item( ent, item );
				ent.client.pers.inventory[ITEM_INDEX( item )]--;
				Cmd.ValidateSelectedItem( ent );
			}
		}

		public static ItemDropAdapter Drop_PowerArmor = new AnonymousItemDropAdapter2();
		private sealed class AnonymousItemDropAdapter2 : ItemDropAdapter
		{
			public override String GetID( )
			{
				return "drop_powerarmor";
			}

			public override void Drop( edict_t ent, gitem_t item )
			{
				if ( 0 != ( ent.flags & Defines.FL_POWER_ARMOR ) && ( ent.client.pers.inventory[ITEM_INDEX( item )] == 1 ) )
					Use_PowerArmor.Use( ent, item );
				Drop_General.Drop( ent, item );
			}
		}

		public static EntThinkAdapter droptofloor = new AnonymousEntThinkAdapter2();
		private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "drop_to_floor";
			}

			public override Boolean Think( edict_t ent )
			{
				trace_t tr;
				Single[] dest = new Single[] { 0, 0, 0 };
				ent.mins[0] = ent.mins[1] = ent.mins[2] = -15;
				ent.maxs[0] = ent.maxs[1] = ent.maxs[2] = 15;
				if ( ent.model != null )
					GameBase.gi.Setmodel( ent, ent.model );
				else
					GameBase.gi.Setmodel( ent, ent.item.world_model );
				ent.solid = Defines.SOLID_TRIGGER;
				ent.movetype = Defines.MOVETYPE_TOSS;
				ent.touch = Touch_Item_Cmd;
				Single[] v = new Single[] { 0, 0, -128 };
				Math3D.VectorAdd( ent.s.origin, v, dest );
				tr = GameBase.gi.Trace( ent.s.origin, ent.mins, ent.maxs, dest, ent, Defines.MASK_SOLID );
				if ( tr.startsolid )
				{
					GameBase.gi.Dprintf( "droptofloor: " + ent.classname + " startsolid at " + Lib.Vtos( ent.s.origin ) + "\\n" );
					GameUtil.G_FreeEdict( ent );
					return true;
				}

				Math3D.VectorCopy( tr.endpos, ent.s.origin );
				if ( ent.team != null )
				{
					ent.flags &= ~Defines.FL_TEAMSLAVE;
					ent.chain = ent.teamchain;
					ent.teamchain = null;
					ent.svflags |= Defines.SVF_NOCLIENT;
					ent.solid = Defines.SOLID_NOT;
					if ( ent == ent.teammaster )
					{
						ent.nextthink = GameBase.level.time + Defines.FRAMETIME;
						ent.think = DoRespawn;
					}
				}

				if ( ( ent.spawnflags & Defines.ITEM_NO_TOUCH ) != 0 )
				{
					ent.solid = Defines.SOLID_BBOX;
					ent.touch = null;
					ent.s.effects &= ~Defines.EF_ROTATE;
					ent.s.renderfx &= ~Defines.RF_GLOW;
				}

				if ( ( ent.spawnflags & Defines.ITEM_TRIGGER_SPAWN ) != 0 )
				{
					ent.svflags |= Defines.SVF_NOCLIENT;
					ent.solid = Defines.SOLID_NOT;
					ent.use = Use_Item_Cmd;
				}

				GameBase.gi.Linkentity( ent );
				return true;
			}
		}

		public static ItemUseAdapter Use_PowerArmor = new AnonymousItemUseAdapter5();
		private sealed class AnonymousItemUseAdapter5 : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "use_powerarmor";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				Int32 index;
				if ( ( ent.flags & Defines.FL_POWER_ARMOR ) != 0 )
				{
					ent.flags &= ~Defines.FL_POWER_ARMOR;
					GameBase.gi.Sound( ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "misc/power2.wav" ), 1, Defines.ATTN_NORM, 0 );
				}
				else
				{
					index = ITEM_INDEX( FindItem( "cells" ) );
					if ( 0 == ent.client.pers.inventory[index] )
					{
						GameBase.gi.Cprintf( ent, Defines.PRINT_HIGH, "No cells for power armor.\\n" );
						return;
					}

					ent.flags |= Defines.FL_POWER_ARMOR;
					GameBase.gi.Sound( ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "misc/power1.wav" ), 1, Defines.ATTN_NORM, 0 );
				}
			}
		}

		public static EntUseAdapter Use_Item_Cmd = new AnonymousEntUseAdapter();
		private sealed class AnonymousEntUseAdapter : EntUseAdapter
		{
			public override String GetID( )
			{
				return "use_item";
			}

			public override void Use( edict_t ent, edict_t other, edict_t activator )
			{
				ent.svflags &= ~Defines.SVF_NOCLIENT;
				ent.use = null;
				if ( ( ent.spawnflags & Defines.ITEM_NO_TOUCH ) != 0 )
				{
					ent.solid = Defines.SOLID_BBOX;
					ent.touch = null;
				}
				else
				{
					ent.solid = Defines.SOLID_TRIGGER;
					ent.touch = Touch_Item_Cmd;
				}

				GameBase.gi.Linkentity( ent );
			}
		}

		public static gitem_t GetItemByIndex( Int32 index )
		{
			if ( index == 0 || index >= GameBase.game.num_items )
				return null;
			return GameItemList.itemlist[index];
		}

		public static gitem_t FindItemByClassname( String classname )
		{
			for ( var i = 1; i < GameBase.game.num_items; i++ )
			{
				gitem_t it = GameItemList.itemlist[i];
				if ( it.classname == null )
					continue;
				if ( it.classname.EqualsIgnoreCase( classname ) )
					return it;
			}

			return null;
		}

		public static gitem_t FindItem( String pickup_name )
		{
			for ( var i = 1; i < GameBase.game.num_items; i++ )
			{
				gitem_t it = GameItemList.itemlist[i];
				if ( it.pickup_name == null )
					continue;
				if ( it.pickup_name.EqualsIgnoreCase( pickup_name ) )
					return it;
			}

			Com.Println( "Item not found:" + pickup_name );
			return null;
		}

		public static void SetRespawn( edict_t ent, Single delay )
		{
			ent.flags = ent.flags | Defines.FL_RESPAWN;
			ent.svflags |= Defines.SVF_NOCLIENT;
			ent.solid = Defines.SOLID_NOT;
			ent.nextthink = GameBase.level.time + delay;
			ent.think = DoRespawn;
			GameBase.gi.Linkentity( ent );
		}

		public static Int32 ITEM_INDEX( gitem_t item )
		{
			return item.index;
		}

		public static edict_t Drop_Item( edict_t ent, gitem_t item )
		{
			edict_t dropped;
			Single[] forward = new Single[] { 0, 0, 0 };
			Single[] right = new Single[] { 0, 0, 0 };
			Single[] offset = new Single[] { 0, 0, 0 };
			dropped = GameUtil.G_Spawn();
			dropped.classname = item.classname;
			dropped.item = item;
			dropped.spawnflags = Defines.DROPPED_ITEM;
			dropped.s.effects = item.world_model_flags;
			dropped.s.renderfx = Defines.RF_GLOW;
			Math3D.VectorSet( dropped.mins, -15, -15, -15 );
			Math3D.VectorSet( dropped.maxs, 15, 15, 15 );
			GameBase.gi.Setmodel( dropped, dropped.item.world_model );
			dropped.solid = Defines.SOLID_TRIGGER;
			dropped.movetype = Defines.MOVETYPE_TOSS;
			dropped.touch = drop_temp_touch;
			dropped.owner = ent;
			if ( ent.client != null )
			{
				trace_t trace;
				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorSet( offset, 24, 0, -16 );
				Math3D.G_ProjectSource( ent.s.origin, offset, forward, right, dropped.s.origin );
				trace = GameBase.gi.Trace( ent.s.origin, dropped.mins, dropped.maxs, dropped.s.origin, ent, Defines.CONTENTS_SOLID );
				Math3D.VectorCopy( trace.endpos, dropped.s.origin );
			}
			else
			{
				Math3D.AngleVectors( ent.s.angles, forward, right, null );
				Math3D.VectorCopy( ent.s.origin, dropped.s.origin );
			}

			Math3D.VectorScale( forward, 100, dropped.velocity );
			dropped.velocity[2] = 300;
			dropped.think = drop_make_touchable;
			dropped.nextthink = GameBase.level.time + 1;
			GameBase.gi.Linkentity( dropped );
			return dropped;
		}

		static void Use_Item( edict_t ent, edict_t other, edict_t activator )
		{
			ent.svflags &= ~Defines.SVF_NOCLIENT;
			ent.use = null;
			if ( ( ent.spawnflags & Defines.ITEM_NO_TOUCH ) != 0 )
			{
				ent.solid = Defines.SOLID_BBOX;
				ent.touch = null;
			}
			else
			{
				ent.solid = Defines.SOLID_TRIGGER;
				ent.touch = Touch_Item_Cmd;
			}

			GameBase.gi.Linkentity( ent );
		}

		public static Int32 PowerArmorType( edict_t ent )
		{
			if ( ent.client == null )
				return Defines.POWER_ARMOR_NONE;
			if ( 0 == ( ent.flags & Defines.FL_POWER_ARMOR ) )
				return Defines.POWER_ARMOR_NONE;
			if ( ent.client.pers.inventory[power_shield_index] > 0 )
				return Defines.POWER_ARMOR_SHIELD;
			if ( ent.client.pers.inventory[power_screen_index] > 0 )
				return Defines.POWER_ARMOR_SCREEN;
			return Defines.POWER_ARMOR_NONE;
		}

		public static Int32 ArmorIndex( edict_t ent )
		{
			if ( ent.client == null )
				return 0;
			if ( ent.client.pers.inventory[jacket_armor_index] > 0 )
				return jacket_armor_index;
			if ( ent.client.pers.inventory[combat_armor_index] > 0 )
				return combat_armor_index;
			if ( ent.client.pers.inventory[body_armor_index] > 0 )
				return body_armor_index;
			return 0;
		}

		public static Boolean Pickup_PowerArmor( edict_t ent, edict_t other )
		{
			Int32 quantity;
			quantity = other.client.pers.inventory[ITEM_INDEX( ent.item )];
			other.client.pers.inventory[ITEM_INDEX( ent.item )]++;
			if ( GameBase.deathmatch.value != 0 )
			{
				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) )
					SetRespawn( ent, ent.item.quantity );
				if ( 0 == quantity )
					ent.item.use.Use( other, ent.item );
			}

			return true;
		}

		public static Boolean Add_Ammo( edict_t ent, gitem_t item, Int32 count )
		{
			Int32 index;
			Int32 max;
			if ( null == ent.client )
				return false;
			if ( item.tag == Defines.AMMO_BULLETS )
				max = ent.client.pers.max_bullets;
			else if ( item.tag == Defines.AMMO_SHELLS )
				max = ent.client.pers.max_shells;
			else if ( item.tag == Defines.AMMO_ROCKETS )
				max = ent.client.pers.max_rockets;
			else if ( item.tag == Defines.AMMO_GRENADES )
				max = ent.client.pers.max_grenades;
			else if ( item.tag == Defines.AMMO_CELLS )
				max = ent.client.pers.max_cells;
			else if ( item.tag == Defines.AMMO_SLUGS )
				max = ent.client.pers.max_slugs;
			else
				return false;
			index = ITEM_INDEX( item );
			if ( ent.client.pers.inventory[index] == max )
				return false;
			ent.client.pers.inventory[index] += count;
			if ( ent.client.pers.inventory[index] > max )
				ent.client.pers.inventory[index] = max;
			return true;
		}

		public static void InitItems( )
		{
			GameBase.game.num_items = GameItemList.itemlist.Length - 1;
		}

		public static void SetItemNames( )
		{
			Int32 i;
			gitem_t it;
			for ( i = 1; i < GameBase.game.num_items; i++ )
			{
				it = GameItemList.itemlist[i];
				GameBase.gi.Configstring( Defines.CS_ITEMS + i, it.pickup_name );
			}

			jacket_armor_index = ITEM_INDEX( FindItem( "Jacket Armor" ) );
			combat_armor_index = ITEM_INDEX( FindItem( "Combat Armor" ) );
			body_armor_index = ITEM_INDEX( FindItem( "Body Armor" ) );
			power_screen_index = ITEM_INDEX( FindItem( "Power Screen" ) );
			power_shield_index = ITEM_INDEX( FindItem( "Power Shield" ) );
		}

		public static void SelectNextItem( edict_t ent, Int32 itflags )
		{
			gclient_t cl;
			Int32 i, index;
			gitem_t it;
			cl = ent.client;
			if ( cl.chase_target != null )
			{
				GameChase.ChaseNext( ent );
				return;
			}

			for ( i = 1; i <= Defines.MAX_ITEMS; i++ )
			{
				index = ( cl.pers.selected_item + i ) % Defines.MAX_ITEMS;
				if ( 0 == cl.pers.inventory[index] )
					continue;
				it = GameItemList.itemlist[index];
				if ( it.use == null )
					continue;
				if ( 0 == ( it.flags & itflags ) )
					continue;
				cl.pers.selected_item = index;
				return;
			}

			cl.pers.selected_item = -1;
		}

		public static void SelectPrevItem( edict_t ent, Int32 itflags )
		{
			gclient_t cl;
			Int32 i, index;
			gitem_t it;
			cl = ent.client;
			if ( cl.chase_target != null )
			{
				GameChase.ChasePrev( ent );
				return;
			}

			for ( i = 1; i <= Defines.MAX_ITEMS; i++ )
			{
				index = ( cl.pers.selected_item + Defines.MAX_ITEMS - i ) % Defines.MAX_ITEMS;
				if ( 0 == cl.pers.inventory[index] )
					continue;
				it = GameItemList.itemlist[index];
				if ( null == it.use )
					continue;
				if ( 0 == ( it.flags & itflags ) )
					continue;
				cl.pers.selected_item = index;
				return;
			}

			cl.pers.selected_item = -1;
		}

		public static void PrecacheItem( gitem_t it )
		{
			String s;
			String data;
			Int32 len;
			gitem_t ammo;
			if ( it == null )
				return;
			if ( it.pickup_sound != null )
				GameBase.gi.Soundindex( it.pickup_sound );
			if ( it.world_model != null )
				GameBase.gi.Modelindex( it.world_model );
			if ( it.view_model != null )
				GameBase.gi.Modelindex( it.view_model );
			if ( it.icon != null )
				GameBase.gi.Imageindex( it.icon );
			if ( it.ammo != null && it.ammo.Length != 0 )
			{
				ammo = FindItem( it.ammo );
				if ( ammo != it )
					PrecacheItem( ammo );
			}

			s = it.precaches;
			if ( s == null || s.Length != 0 )
				return;
			StringTokenizer tk = new StringTokenizer( s );
			while ( tk.RemainingTokens > 0 )
			{
				tk.MoveNext();
				data = tk.Current;
				len = data.Length;
				if ( len >= Defines.MAX_QPATH || len < 5 )
					GameBase.gi.Error( "PrecacheItem: it.classname has bad precache string: " + s );
				if ( data.EndsWith( "md2" ) )
					GameBase.gi.Modelindex( data );
				else if ( data.EndsWith( "sp2" ) )
					GameBase.gi.Modelindex( data );
				else if ( data.EndsWith( "wav" ) )
					GameBase.gi.Soundindex( data );
				else if ( data.EndsWith( "pcx" ) )
					GameBase.gi.Imageindex( data );
				else
					GameBase.gi.Error( "PrecacheItem: bad precache string: " + data );
			}
		}

		public static void SpawnItem( edict_t ent, gitem_t item )
		{
			PrecacheItem( item );
			if ( ent.spawnflags != 0 )
			{
				if ( Lib.Strcmp( ent.classname, "key_power_cube" ) != 0 )
				{
					ent.spawnflags = 0;
					GameBase.gi.Dprintf( "" + ent.classname + " at " + Lib.Vtos( ent.s.origin ) + " has invalid spawnflags set\\n" );
				}
			}

			if ( GameBase.deathmatch.value != 0 )
			{
				if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_ARMOR ) != 0 )
				{
					if ( item.pickup == Pickup_Armor || item.pickup == Pickup_PowerArmor_Cmd )
					{
						GameUtil.G_FreeEdict( ent );
						return;
					}
				}

				if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_ITEMS ) != 0 )
				{
					if ( item.pickup == Pickup_Powerup )
					{
						GameUtil.G_FreeEdict( ent );
						return;
					}
				}

				if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_HEALTH ) != 0 )
				{
					if ( item.pickup == Pickup_Health || item.pickup == Pickup_Adrenaline || item.pickup == Pickup_AncientHead )
					{
						GameUtil.G_FreeEdict( ent );
						return;
					}
				}

				if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) != 0 )
				{
					if ( ( item.flags == Defines.IT_AMMO ) || ( Lib.Strcmp( ent.classname, "weapon_bfg" ) == 0 ) )
					{
						GameUtil.G_FreeEdict( ent );
						return;
					}
				}
			}

			if ( GameBase.coop.value != 0 && ( Lib.Strcmp( ent.classname, "key_power_cube" ) == 0 ) )
			{
				ent.spawnflags |= ( 1 << ( 8 + GameBase.level.power_cubes ) );
				GameBase.level.power_cubes++;
			}

			if ( ( GameBase.coop.value != 0 ) && ( item.flags & Defines.IT_STAY_COOP ) != 0 )
			{
				item.drop = null;
			}

			ent.item = item;
			ent.nextthink = GameBase.level.time + 2 * Defines.FRAMETIME;
			ent.think = droptofloor;
			ent.s.effects = item.world_model_flags;
			ent.s.renderfx = Defines.RF_GLOW;
			if ( ent.model != null )
				GameBase.gi.Modelindex( ent.model );
		}

		public static void SP_item_health( edict_t self )
		{
			if ( GameBase.deathmatch.value != 0 && ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_HEALTH ) != 0 )
			{
				GameUtil.G_FreeEdict( self );
			}

			self.model = "models/items/healing/medium/tris.md2";
			self.count = 10;
			SpawnItem( self, FindItem( "Health" ) );
			GameBase.gi.Soundindex( "items/n_health.wav" );
		}

		public static void SP_item_health_small( edict_t self )
		{
			if ( GameBase.deathmatch.value != 0 && ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_HEALTH ) != 0 )
			{
				GameUtil.G_FreeEdict( self );
				return;
			}

			self.model = "models/items/healing/stimpack/tris.md2";
			self.count = 2;
			SpawnItem( self, FindItem( "Health" ) );
			self.style = Defines.HEALTH_IGNORE_MAX;
			GameBase.gi.Soundindex( "items/s_health.wav" );
		}

		public static void SP_item_health_large( edict_t self )
		{
			if ( GameBase.deathmatch.value != 0 && ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_HEALTH ) != 0 )
			{
				GameUtil.G_FreeEdict( self );
				return;
			}

			self.model = "models/items/healing/large/tris.md2";
			self.count = 25;
			SpawnItem( self, FindItem( "Health" ) );
			GameBase.gi.Soundindex( "items/l_health.wav" );
		}

		public static void SP_item_health_mega( edict_t self )
		{
			if ( GameBase.deathmatch.value != 0 && ( ( Int32 ) GameBase.dmflags.value & Defines.DF_NO_HEALTH ) != 0 )
			{
				GameUtil.G_FreeEdict( self );
				return;
			}

			self.model = "models/items/mega_h/tris.md2";
			self.count = 100;
			SpawnItem( self, FindItem( "Health" ) );
			GameBase.gi.Soundindex( "items/m_health.wav" );
			self.style = Defines.HEALTH_IGNORE_MAX | Defines.HEALTH_TIMED;
		}

		public static void Touch_Item( edict_t ent, edict_t other, cplane_t plane, csurface_t surf )
		{
			Boolean taken;
			if ( other.client == null || ent.item == null )
				return;
			if ( other.health < 1 )
				return;
			if ( ent.item.pickup == null )
				return;
			taken = ent.item.pickup.Interact( ent, other );
			if ( taken )
			{
				other.client.bonus_alpha = 0.25F;
				other.client.ps.stats[Defines.STAT_PICKUP_ICON] = ( Int16 ) GameBase.gi.Imageindex( ent.item.icon );
				other.client.ps.stats[Defines.STAT_PICKUP_STRING] = ( Int16 ) ( Defines.CS_ITEMS + ITEM_INDEX( ent.item ) );
				other.client.pickup_msg_time = GameBase.level.time + 3F;
				if ( ent.item.use != null )
					other.client.pers.selected_item = other.client.ps.stats[Defines.STAT_SELECTED_ITEM] = ( Int16 ) ITEM_INDEX( ent.item );
				if ( ent.item.pickup == Pickup_Health )
				{
					if ( ent.count == 2 )
						GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/s_health.wav" ), 1, Defines.ATTN_NORM, 0 );
					else if ( ent.count == 10 )
						GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/n_health.wav" ), 1, Defines.ATTN_NORM, 0 );
					else if ( ent.count == 25 )
						GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/l_health.wav" ), 1, Defines.ATTN_NORM, 0 );
					else
						GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/m_health.wav" ), 1, Defines.ATTN_NORM, 0 );
				}
				else if ( ent.item.pickup_sound != null )
				{
					GameBase.gi.Sound( other, Defines.CHAN_ITEM, GameBase.gi.Soundindex( ent.item.pickup_sound ), 1, Defines.ATTN_NORM, 0 );
				}
			}

			if ( 0 == ( ent.spawnflags & Defines.ITEM_TARGETS_USED ) )
			{
				GameUtil.G_UseTargets( ent, other );
				ent.spawnflags |= Defines.ITEM_TARGETS_USED;
			}

			if ( !taken )
				return;
			if ( !( ( GameBase.coop.value != 0 ) && ( ent.item.flags & Defines.IT_STAY_COOP ) != 0 ) || 0 != ( ent.spawnflags & ( Defines.DROPPED_ITEM | Defines.DROPPED_PLAYER_ITEM ) ) )
			{
				if ( ( ent.flags & Defines.FL_RESPAWN ) != 0 )
					ent.flags = ent.flags & ~Defines.FL_RESPAWN;
				else
					GameUtil.G_FreeEdict( ent );
			}
		}
	}
}